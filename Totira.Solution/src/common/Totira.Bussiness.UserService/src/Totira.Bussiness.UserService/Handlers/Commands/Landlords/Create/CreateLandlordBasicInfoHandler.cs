using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Events.Landlord.CreatedEvents;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Create
{
    public class CreateLandlordBasicInfoHandler : IMessageHandler<CreateLandlordBasicInfoCommand>
    {
        private readonly IRepository<LandlordBasicInformation, Guid> _landlordBasicInformationRepository;
        private readonly ILogger<CreateLandlordBasicInfoHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateLandlordBasicInfoHandler(
            IRepository<LandlordBasicInformation, Guid> landlordBasicInformationRepository,
            ILogger<CreateLandlordBasicInfoHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _landlordBasicInformationRepository = landlordBasicInformationRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateLandlordBasicInfoCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogInformation("creating landlord basic information with id {landlordId}", cmd.LandlordId);

                var birthday = cmd.Birthday is null
                    ? default
                    :   Birthday.From(
                        cmd.Birthday.Year,
                        cmd.Birthday.Day,
                        cmd.Birthday.Month);

                var landlodBasicInformation = LandlordBasicInformation.Create(
                    cmd.LandlordId, 
                    cmd.FirstName,
                    cmd.LastName,
                    cmd.Email,
                    birthday,
                    cmd.SocialInsuranceNumber,
                    cmd.AboutMe);

                await _landlordBasicInformationRepository.Add(landlodBasicInformation);

                var userCreatedEvent = new LandlordBasicInformationCreatedEvent(landlodBasicInformation.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
            }, async ex =>
            {
                var userCreatedEvent = new LandlordBasicInformationCreatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
                throw ex;
            });
        }
    }
}