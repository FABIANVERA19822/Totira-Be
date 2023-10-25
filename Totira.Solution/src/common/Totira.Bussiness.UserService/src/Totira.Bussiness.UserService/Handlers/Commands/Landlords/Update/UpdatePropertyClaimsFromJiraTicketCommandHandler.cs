namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Totira.Bussiness.UserService.Commands.LandlordCommands;
    using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
    using Totira.Bussiness.UserService.Commands.LandlordCommands.Update;
    using Totira.Bussiness.UserService.EmailTemplates;
    using Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents;
    using Totira.Support.Application.Messages;
    using Totira.Support.CommonLibrary.Interfaces;
    using Totira.Support.CommonLibrary.Settings;
    using Totira.Support.EventServiceBus;
    using Totira.Support.Otp;

    public class UpdatePropertyClaimsFromJiraTicketCommandHandler : BaseMessageHandler<UpdatePropertyClaimsFromJiraTicketCommand, UpdatePropertyClaimsFromJiraTicketEvent>
    {
        private readonly IOtpHandler _otpHandler;
        private readonly IEventBus _bus;
        private readonly FrontendSettings _settings;
        private readonly IEmailHandler _emailHandler;

        public UpdatePropertyClaimsFromJiraTicketCommandHandler(
            ILogger<UpdatePropertyClaimsFromJiraTicketCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService,
            IOtpHandler otpHandler,
            IOptions<FrontendSettings> settings,
            IEventBus bus,
            IEmailHandler emailHandler
            )
            : base(logger, contextFactory, messageService)
        {
            _otpHandler = otpHandler;
            _bus = bus;
            _settings = settings.Value;
            _emailHandler = emailHandler;
        }

        protected override async Task<UpdatePropertyClaimsFromJiraTicketEvent> Process(IContext context, UpdatePropertyClaimsFromJiraTicketCommand command)
        {
            var otpId = Guid.NewGuid();
            await _otpHandler.SetOtpProcessAsync(otpId, command.LandlordId, command.ClaimId, command.Email, command.Email, "PropertyClaim", true, false);
            var link = EmailHelper.BuildLandLordPropertyClaimLink(_settings.Url, otpId);
            var emailBody = string.Empty;
            var subject = "Property ownership request";
            if (command.Status == "Approved")
            {
                emailBody = EmailHelper.BuildApprovedPropertyOwnership(command.Address, link);
            }
            else
            {
                emailBody = EmailHelper.BuildRejectedPropertyOwnership(command.Address, link, command.Message);
            }
            await SendEmailAsync(command.Email, subject, emailBody);
            await UpdatePropertyClaimsFromJiraTicket(command.ClaimId, command.Status, command.Message, command.MLSId);
            await AssignProppertyToLandlordViaClaim(command.ClaimId, command.MLSId);
            return new UpdatePropertyClaimsFromJiraTicketEvent()
            {
                ClaimId = command.ClaimId,
                Id = command.LandlordId,
                Status = command.Status,
                Message = command.Message
            };
        }

        private async Task SendEmailAsync(string email, string subject, string emailBody)
        {
            var isSent = await _emailHandler.SendEmailAsync(email, subject, emailBody);

            if (!isSent)
                _logger.LogError("Fail sending email.");
        }

        private async Task UpdatePropertyClaimsFromJiraTicket(Guid claimId, string status, string reason, string mlsId)
        {
            var updateClaimJiraTicketResult = new UpdateClaimJiraTicketResultCommand()
            {
                ClaimId = claimId,
                Status = status,
                Reason = reason,
                MLSId = mlsId,
            };
            var userContext = _contextFactory.Create(string.Empty, claimId);

            await _bus.PublishAsync(userContext, updateClaimJiraTicketResult);
        }

        private async Task AssignProppertyToLandlordViaClaim(Guid claimId, string mlsId)
        {
            var propertyDisplayCommand = new CreateLandlordPropertyDisplayCommand()
            {
                ClaimId = claimId,
                MLSId = mlsId
            };

            var thirdpartyContext = _contextFactory.Create(string.Empty, claimId);

            await _bus.PublishAsync(thirdpartyContext, propertyDisplayCommand);

        }
    }
}