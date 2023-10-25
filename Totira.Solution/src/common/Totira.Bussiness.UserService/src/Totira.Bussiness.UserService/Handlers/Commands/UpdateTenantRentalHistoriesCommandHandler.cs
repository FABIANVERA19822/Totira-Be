using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantRentalHistoriesCommandHandler : IMessageHandler<UpdateTenantRentalHistoriesCommand>
    {
        private readonly ILogger<UpdateTenantRentalHistoriesCommandHandler> _logger;
        private readonly IRepository<TenantRentalHistories, Guid> _TenantRentalHistoriesRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenatBasicInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IOtpHandler _otpHandler;

        public UpdateTenantRentalHistoriesCommandHandler(
            IRepository<TenantRentalHistories, Guid> TenantRentalHistoriesRepository,
            ILogger<UpdateTenantRentalHistoriesCommandHandler> logger,
            IRepository<TenantBasicInformation, Guid> tenatBasicInformationRepository,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings,
            IContextFactory contextFactory,
            IMessageService messageService,
            IOtpHandler otpHandler)
        {
            _logger = logger;
            _TenantRentalHistoriesRepository = TenantRentalHistoriesRepository;
            _emailHandler = emailHandler;
            _tenatBasicInformationRepository = tenatBasicInformationRepository;
            _settings = settings;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantRentalHistoriesCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
            _logger.LogDebug("Update tenant rental history for tenant {TenantId}", cmd.TenantId);

            var actualData = await _TenantRentalHistoriesRepository.GetByIdAsync(cmd.TenantId);
            var landlordId = Guid.NewGuid();

            var info = await _tenatBasicInformationRepository.GetByIdAsync(cmd.TenantId);
            if (info is not null)
            {
                var tenantName = $"{info?.FirstName} {info?.LastName}";

                if (actualData.RentalHistories!.Count() < 10)
                {
                    actualData.RentalHistories!.Add(new TenantRentalHistory()
                    {
                        Id = landlordId,
                        TenantId = cmd.TenantId,
                        FullName = tenantName,
                        RentalStartDate = new Domain.CustomDate(cmd.RentalStartDate!.Month, cmd.RentalStartDate!.Year),
                        CurrentlyLivingHere = cmd.CurrentlyLivingHere,
                        RentalEndDate = new Domain.CustomDate(cmd.RentalEndDate!.Month, cmd.RentalEndDate!.Year),
                        Country = cmd.Country,
                        State = cmd.State,
                        City = cmd.City,
                        Address = cmd.Address,
                        Unit = cmd.Unit,
                        ZipCode = cmd.ZipCode,
                        IsFeedbackRequest = cmd.IsFeedbackRequest,
                        Status = cmd.IsFeedbackRequest ? "Requested" : "Created",
                        CreatedOn = DateTimeOffset.Now,
                        ContactInformation = new Domain.RentalHistoryLandlordContactInformation(cmd.ContactInformation!.Relationship,
                        cmd.ContactInformation!.FirstName, cmd.ContactInformation!.LastName, new Domain.RentalHistoriesPhoneNumber(cmd.ContactInformation!.PhoneNumber.Number, cmd.ContactInformation!.PhoneNumber.CountryCode),
                        cmd.ContactInformation!.EmailAddress)
                    });

                        await _TenantRentalHistoriesRepository.Update(actualData);

                        var tenantUpdatedEvent = new TenantRentalHistoriesUpdatedEvent(actualData.Id);
                        var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                        var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);

                        if (cmd.IsFeedbackRequest)
                        {
                            string htmlEmail = EmailTemplateResource.ReferralLandlordTemplate;
                            var landlordName = $"{cmd.ContactInformation.FirstName} {cmd.ContactInformation.LastName}";
                            var contactInformation = string.Empty;
                            var otpId = Guid.NewGuid();


                            await _otpHandler.SetOtpProcessAsync(otpId, landlordId, cmd.TenantId, cmd.ContactInformation.EmailAddress, cmd.ContactInformation.EmailAddress, "Feedback", true, false);
                            var link = EmailHelper.BuildReferralLandlordfeedbackOtpLink(_settings.Value.Url, otpId);

                            htmlEmail = htmlEmail
                                .Replace("[landlordName]", landlordName)
                                .Replace("[tenantName]", tenantName)
                                .Replace("[link]", link)
                                .Replace("[contactInformation]", contactInformation);

                            var emailResult = await _emailHandler.SendEmailAsync(cmd.ContactInformation.EmailAddress, "Request for Tenant Feedback", htmlEmail);
                            if (!emailResult)
                                _logger.LogError("fail sendingEmail");
                        }
                    }
                }
            }, ex => throw ex);
        }


    }
}