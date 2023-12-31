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

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class CreateTenantShareProfileCommandHandler : IMessageHandler<CreateTenantShareProfileCommand>
{
    private readonly ILogger<CreateTenantShareProfileCommandHandler> _logger;
    private readonly IRepository<TenantShareProfile, Guid> _tenantShareProfileRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly IOptions<FrontendSettings> _settings;
    private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
    private readonly IEncryptionHandler _encryptionHandler;
    public CreateTenantShareProfileCommandHandler(ILogger<CreateTenantShareProfileCommandHandler> logger,
        IRepository<TenantShareProfile, Guid> tenantShareProfileRepository,
        IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
          IEmailHandler emailHandler,
        IOptions<FrontendSettings> settings, IEncryptionHandler encryptionHandler)
    {
        _logger = logger;
        _tenantShareProfileRepository = tenantShareProfileRepository;
        _emailHandler = emailHandler;
        _settings = settings;
        _tenantBasicInformationRepository = tenantBasicInformationRepository;
        _encryptionHandler = encryptionHandler;
    }

    public async Task HandleAsync(IContext context, CreateTenantShareProfileCommand command)
    {
        _logger.LogDebug($"creating tenant Share Profile for tenant {command.TenantId}");

        Random objRandom = new Random();
        int accessCode = objRandom.Next(100000, 999999);
        string encryptedAccessCode = _encryptionHandler.EncryptString(accessCode.ToString());

        var tenantShareProfile = new TenantShareProfile
        {
            Id = Guid.NewGuid(),
            TenantId = command.TenantId,
            Email = command.Email,
            TypeOfContact = command.TypeOfContact,
            PropertyStreetAddress = command.PropertyStreetAddress,
            Message = command.Message,
            EncryptedAccessCode = encryptedAccessCode,
        };
        await _tenantShareProfileRepository.Add(tenantShareProfile);
        var userCreatedEvent = new TenantEmploymentReferenceCreatedEvent(tenantShareProfile.Id);

        SendEmail(command.Email, accessCode.ToString(), command.TenantId, command.Message,command.PropertyStreetAddress);
    }


    #region Helper Methods
    async void SendEmail(string email, string accessCode, Guid tenantId, string? customizeMessage,string? propertyStreetAddress)
    {

        var tenant = await _tenantBasicInformationRepository.GetByIdAsync(tenantId);

        if (tenant is not null)
        {
            string link = EmailHelper.BuildProfileSharingLink(baseUrl: _settings.Value.Url, tenantId: tenantId,email: email);

            string tenantFullName = $"{tenant?.FirstName} {tenant?.LastName}";
            var emailBody = EmailHelper.BuildProfileSharingEmailBody(accessCode, tenantFullName, link, tenant?.FirstName, customizeMessage,tenant?.FirstName.Substring(0, 1).ToUpper(), propertyStreetAddress);

            string subject = "Our client is interested in your property";
            var isSent = await _emailHandler.SendEmailAsync(email, subject, emailBody);

            if (!isSent)
                _logger.LogError("Fail sending email.");

        }
    }
    #endregion
}

