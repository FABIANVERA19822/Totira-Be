using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantRentalHistoriesCommandHandler : IMessageHandler<CreateTenantRentalHistoriesCommand>
    {
        private readonly ILogger<CreateTenantRentalHistoriesCommandHandler> _logger;
        private readonly IRepository<TenantRentalHistories, Guid> _TenantRentalHistoriesRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenatBasicInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        private readonly IOtpHandler _otpHandler;

        public CreateTenantRentalHistoriesCommandHandler(ILogger<CreateTenantRentalHistoriesCommandHandler> logger,
           IRepository<TenantRentalHistories, Guid> TenantRentalHistoriesRepository,
           IRepository<TenantBasicInformation, Guid> tenatBasicInformationRepository,
           IEmailHandler emailHandler,
           IOptions<FrontendSettings> settings,
           IOtpHandler otpHandler)
        {
            _logger = logger;
            _TenantRentalHistoriesRepository = TenantRentalHistoriesRepository;
            _tenatBasicInformationRepository = tenatBasicInformationRepository;
            _emailHandler = emailHandler;
            _settings = settings;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantRentalHistoriesCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogDebug("creating tenant rental history for tenant {TenantId}", cmd.TenantId);
                var landlordId = Guid.NewGuid();

                var actualData = await _TenantRentalHistoriesRepository.GetByIdAsync(cmd.TenantId);

                var info = await _tenatBasicInformationRepository.GetByIdAsync(cmd.TenantId);

                if (actualData == null)
                {
                    await CreateRentalHistories(cmd, landlordId, info);
                }
                else
                {
                    await UpdateTenantRentalHistories(cmd, landlordId, info, actualData);
                }
                if (cmd.IsFeedbackRequest)
                {
                    await SendFeedbackRequestMail(cmd, landlordId, info);
                }

                return true;
            }, (Func<Exception, bool>)(ex => throw ex));
        }

        private async Task SendFeedbackRequestMail(CreateTenantRentalHistoriesCommand command, Guid landlordId, TenantBasicInformation info)
        {
            string htmlEmail = EmailTemplateResource.ReferralLandlordTemplate;
            var landlordName = $"{command.ContactInformation.FirstName} {command.ContactInformation.LastName}";
            var tenantName = $"{info.FirstName} {info.LastName}";
            var contactInformation = string.Empty;
            var otpId = Guid.NewGuid();

            await _otpHandler.SetOtpProcessAsync(otpId, landlordId, command.TenantId, command.ContactInformation.EmailAddress, command.ContactInformation.EmailAddress, "Feedback", true, false);
            var link = EmailHelper.BuildReferralLandlordfeedbackOtpLink(_settings.Value.Url, otpId);
            htmlEmail = htmlEmail
                .Replace("[tenantName]", tenantName)
                .Replace("[landlordName]", landlordName)
                .Replace("[link]", link)
                .Replace("[contactInformation]", contactInformation);

            var emailResult = await _emailHandler.SendEmailAsync(command.ContactInformation.EmailAddress, "Request for Tenant Feedback", htmlEmail);
            if (!emailResult)
                _logger.LogError("fail sendingEmail");
        }

        private async Task UpdateTenantRentalHistories(CreateTenantRentalHistoriesCommand command, Guid landlordId, TenantBasicInformation info, TenantRentalHistories actualData)
        {
            if (actualData.RentalHistories!.Count() < 10)
            {
                actualData.RentalHistories!.Add(new TenantRentalHistory()
                {
                    Id = landlordId,
                    TenantId = command.TenantId,
                    FullName = $"{info.FirstName} {info.LastName}",
                    RentalStartDate = new Domain.CustomDate(command.RentalStartDate!.Month, command.RentalStartDate!.Year),
                    CurrentlyLivingHere = command.CurrentlyLivingHere,
                    RentalEndDate = new Domain.CustomDate(command.RentalEndDate!.Month, command.RentalEndDate!.Year),
                    Country = command.Country,
                    State = command.State,
                    City = command.City,
                    Address = command.Address,
                    Unit = command.Unit,
                    ZipCode = command.ZipCode,
                    IsFeedbackRequest = command.IsFeedbackRequest,
                    Status = command.IsFeedbackRequest ? "Requested" : "Created",
                    CreatedOn = DateTimeOffset.Now,
                    ContactInformation = new Domain.RentalHistoryLandlordContactInformation(command.ContactInformation!.Relationship,
                    command.ContactInformation!.FirstName, command.ContactInformation!.LastName, new Domain.RentalHistoriesPhoneNumber(command.ContactInformation!.PhoneNumber.Number, command.ContactInformation!.PhoneNumber.CountryCode),
                    command.ContactInformation!.EmailAddress)
                });
                await _TenantRentalHistoriesRepository.Update(actualData);
            }
        }

        private async Task CreateRentalHistories(CreateTenantRentalHistoriesCommand command, Guid tenantId, TenantBasicInformation info)
        {
            var rentalHistory = new List<TenantRentalHistory>();
            rentalHistory.Add(new TenantRentalHistory()
            {
                Id = tenantId,
                TenantId = command.TenantId,
                FullName = $"{info.FirstName} {info.LastName}",
                RentalStartDate = new Domain.CustomDate(command.RentalStartDate!.Month, command.RentalStartDate!.Year),
                CurrentlyLivingHere = command.CurrentlyLivingHere,
                RentalEndDate = new Domain.CustomDate(command.RentalEndDate!.Month, command.RentalEndDate!.Year),
                Country = command.Country,
                State = command.State,
                City = command.City,
                Address = command.Address,
                Unit = command.Unit,
                ZipCode = command.ZipCode,
                CreatedOn = DateTimeOffset.Now,
                IsFeedbackRequest = command.IsFeedbackRequest,
                Status = command.IsFeedbackRequest ? "Requested" : "Created",
                ContactInformation = new RentalHistoryLandlordContactInformation(command.ContactInformation!.Relationship,
                                                                                 command.ContactInformation!.FirstName,
                                                                                 command.ContactInformation!.LastName,
                                                                                 new Domain.RentalHistoriesPhoneNumber(command.ContactInformation!.PhoneNumber.Number,
                                                                                                                       command.ContactInformation!.PhoneNumber.CountryCode),
                command.ContactInformation!.EmailAddress)
            });

            var tenantRentalHistories = new TenantRentalHistories()
            {
                Id = command.TenantId,
                RentalHistories = rentalHistory
            };
            var tenant = new TenantBasicInformation()
            {
                Id = command.TenantId
            };

            await _TenantRentalHistoriesRepository.Add(tenantRentalHistories);
        }
    }
}