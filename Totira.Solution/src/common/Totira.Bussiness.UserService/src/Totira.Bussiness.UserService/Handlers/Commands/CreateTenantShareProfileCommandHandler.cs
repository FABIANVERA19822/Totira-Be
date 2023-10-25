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

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class CreateTenantShareProfileCommandHandler : IMessageHandler<CreateTenantShareProfileCommand>
{
    private readonly ILogger<CreateTenantShareProfileCommandHandler> _logger;
    private readonly IRepository<TenantShareProfile, Guid> _tenantShareProfileRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly IOptions<FrontendSettings> _settings;
    private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
    private readonly IEncryptionHandler _encryptionHandler;
    private readonly IOtpHandler _otpHandler;

    public CreateTenantShareProfileCommandHandler(ILogger<CreateTenantShareProfileCommandHandler> logger,
        IRepository<TenantShareProfile, Guid> tenantShareProfileRepository,
        IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
        IEmailHandler emailHandler,
        IOptions<FrontendSettings> settings,
        IEncryptionHandler encryptionHandler,
        IOtpHandler otpHandler)
    {
        _logger = logger;
        _tenantShareProfileRepository = tenantShareProfileRepository;
        _emailHandler = emailHandler;
        _settings = settings;
        _tenantBasicInformationRepository = tenantBasicInformationRepository;
        _encryptionHandler = encryptionHandler;
        _otpHandler = otpHandler;
    }

    public async Task HandleAsync(IContext context, Either<Exception, CreateTenantShareProfileCommand> command)
    {
        await command.MatchAsync(async cmd =>
        {
            _logger.LogDebug("creating tenant Share Profile for tenant {TenantId}", cmd.TenantId);

            Random objRandom = new Random();
            int accessCode = objRandom.Next(100000, 999999);
            var otpId = Guid.NewGuid();
            string encryptedAccessCode = _encryptionHandler.EncryptString(accessCode.ToString());

            DeleteAllLastShareProfile(cmd.TenantId, cmd.Email);

            var tenantShareProfile = new TenantShareProfile
            {
                Id = Guid.NewGuid(),
                TenantId = cmd.TenantId,
                Email = cmd.Email,
                TypeOfContact = cmd.TypeOfContact,
                PropertyStreetAddress = cmd.PropertyStreetAddress,
                Message = cmd.Message,
                EncryptedAccessCode = encryptedAccessCode,
            };
            await _tenantShareProfileRepository.Add(tenantShareProfile);

            await _otpHandler.SetOtpProcessAsync(otpId, cmd.TenantId, cmd.Email, encryptedAccessCode, "SingleTenant", false, true);
            var userCreatedEvent = new TenantEmploymentReferenceCreatedEvent(tenantShareProfile.Id);

            SendEmail(cmd.Email, accessCode.ToString(), cmd.TenantId, otpId, cmd.Message, cmd.PropertyStreetAddress);
        }, ex => throw ex);
    }

    #region Helper Methods

    private async void DeleteAllLastShareProfile(Guid tenantId, string email)
    {
        var lastShareProfile = await _tenantShareProfileRepository.Get(x => x.TenantId == tenantId && x.Email == email);
        if (lastShareProfile.Any())
        {
            foreach (var item in lastShareProfile)
            {
                await _tenantShareProfileRepository.Delete(item);
            }
        }
    }

    private async void SendEmail(string email, string accessCode, Guid tenantId, Guid otpId, string? customizeMessage, string? propertyStreetAddress)
    {
        var tenant = await _tenantBasicInformationRepository.GetByIdAsync(tenantId);

        if (tenant is not null)
        {
            var link = EmailHelper.BuildProfileSharingOtpLink(baseUrl: _settings.Value.Url, otpId: otpId);

            string tenantFullName = $"{tenant?.FirstName} {tenant?.LastName}";
            var emailBody = EmailHelper.BuildProfileSharingEmailBody(accessCode, tenantFullName, link, tenant?.FirstName, customizeMessage, tenant?.FirstName.Substring(0, 1).ToUpper(), propertyStreetAddress);

            string subject = "Our client is interested in your property";
            var isSent = await _emailHandler.SendEmailAsync(email, subject, emailBody);

            if (!isSent)
                _logger.LogError("Fail sending email.");
        }
    }

    #endregion Helper Methods
}