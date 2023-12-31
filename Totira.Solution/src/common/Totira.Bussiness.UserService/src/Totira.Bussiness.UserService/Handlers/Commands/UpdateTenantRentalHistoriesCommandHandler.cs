﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantRentalHistoriesCommandHandler : IMessageHandler<UpdateTenantRentalHistoriesCommand>
    {
        private readonly ILogger<UpdateTenantRentalHistoriesCommandHandler> _logger;
        private readonly IRepository<TenantRentalHistories, Guid> _TenantRentalHistoriesRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenatPersonalInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        public UpdateTenantRentalHistoriesCommandHandler(
            IRepository<TenantRentalHistories, Guid> TenantRentalHistoriesRepository,
            ILogger<UpdateTenantRentalHistoriesCommandHandler> logger,
            IRepository<TenantBasicInformation, Guid> tenatPersonalInformationRepository,
            IEmailHandler emailHandler,
             IOptions<FrontendSettings> settings)
        {
            _logger = logger;
            _TenantRentalHistoriesRepository = TenantRentalHistoriesRepository;
            _emailHandler = emailHandler;
            _tenatPersonalInformationRepository = tenatPersonalInformationRepository;
            _settings = settings;
        }

        public async Task HandleAsync(IContext context, UpdateTenantRentalHistoriesCommand command)
        {
            _logger.LogDebug($"Update tenant rental history for tenant{command.TenantId}");

            var actualData = await _TenantRentalHistoriesRepository.GetByIdAsync(command.TenantId);
            var landlordId = Guid.NewGuid();

            var info = await _tenatPersonalInformationRepository.GetByIdAsync(command.TenantId);
            if (info is not null)
            {
                var tenantName = $"{info?.FirstName} {info?.LastName}";

                if (actualData.RentalHistories!.Count() < 10)
                {

                    actualData.RentalHistories!.Add(new TenantRentalHistory()
                    {
                        Id = landlordId,
                        TenantId = command.TenantId,
                        FullName = tenantName,
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
                        ContactInformation = new Domain.LandlordContactInformation(command.ContactInformation!.Relationship,
                        command.ContactInformation!.FirstName, command.ContactInformation!.LastName, new Domain.RentalHistoriesPhoneNumber(command.ContactInformation!.PhoneNumber.Number, command.ContactInformation!.PhoneNumber.CountryCode),
                        command.ContactInformation!.EmailAddress)
                    });

                    await _TenantRentalHistoriesRepository.Update(actualData);

                    var tenantUpdatedEvent = new TenantRentalHistoriesUpdatedEvent(actualData.Id);


                    if (command.IsFeedbackRequest)
                    {
                        string htmlEmail = EmailTemplateResource.ReferralLandlordTemplate;
                        var landlordName = $"{command.ContactInformation.FirstName} {command.ContactInformation.LastName}";
                        var contactInformation = string.Empty;
                        var link = $"{_settings.Value.Url}/feedback?landlordId={landlordId}&tenantId={command.TenantId}";
                        htmlEmail = htmlEmail
                            .Replace("[landlordName]", landlordName)
                            .Replace("[tenantName]", tenantName)
                            .Replace("[link]", link)
                            .Replace("[contactInformation]", contactInformation);

                        var emailResult = await _emailHandler.SendEmailAsync(command.ContactInformation.EmailAddress, "Request for Tenant Feedback", htmlEmail);
                        if (!emailResult)
                            _logger.LogError("fail sendingEmail");


                    }
                }
            }
        }
    }
}

