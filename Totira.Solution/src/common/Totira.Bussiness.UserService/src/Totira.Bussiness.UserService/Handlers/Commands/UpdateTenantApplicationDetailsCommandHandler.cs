using LanguageExt;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantApplicationDetailsCommandHandler : IMessageHandler<UpdateTenantApplicationDetailsCommand>
    {
        private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;
        private readonly ILogger<UpdateTenantApplicationDetailsCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public UpdateTenantApplicationDetailsCommandHandler(
            IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
            ILogger<UpdateTenantApplicationDetailsCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantApplicationDetailsCommand> command)
        {
            await command.MatchAsync(async cmd => 
            {
                _logger.LogDebug("Updating tenant personal information with id {Id}", cmd.Id);


                Expression<Func<TenantApplicationDetails, bool>> expression = ad => ad.TenantId == cmd.Id;

                var response = await _tenantApplicationDetailsRepository.Get(expression);

                var actualData = response.MaxBy(x => x.CreatedOn);

                if (actualData != null)
                {
                    if (cmd.IsVerificationsRequested.HasValue && cmd.IsVerificationsRequested.Value)
                    {
                        actualData.IsVerificationsRequested = cmd.IsVerificationsRequested.Value;
                        await _tenantApplicationDetailsRepository.Update(actualData);
                        var tenantVerificationUpdatedEvent = new TenantBasicInformationUpdatedEvent(actualData.Id);
                        var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                        var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantVerificationUpdatedEvent);
                        return;
                    }
                    else
                    {
                        if (cmd.EstimatedMove != null)
                        {
                            var estimatedMove = new Domain.ApplicationDetailEstimatedMove(cmd.EstimatedMove.Month, cmd.EstimatedMove.Year);
                            actualData.EstimatedMove = estimatedMove;
                        }

                        if (actualData.Smoker != cmd.Smoker)
                        {
                            actualData.Smoker = cmd.Smoker;
                        }

                        if (actualData.EstimatedRent != cmd.EstimatedRent && cmd.EstimatedRent != string.Empty)
                        {
                            actualData.EstimatedRent = cmd.EstimatedRent;
                        }

                        if (actualData.Occupants.Adults != cmd.Occupants.Adults)
                        {
                            actualData.Occupants.Adults = cmd.Occupants.Adults;
                        }

                        if (actualData.Occupants.Childrens != cmd.Occupants.Children)
                        {
                            actualData.Occupants.Childrens = cmd.Occupants.Children;
                        }

                        if (cmd.Cars.Count > 0)
                        {
                            if (cmd.Cars != null && cmd.Cars.Any())
                            {
                                List<Domain.ApplicationDetailCar> cars = new List<Domain.ApplicationDetailCar>();
                                cmd.Cars.ForEach(c => cars.Add(new Domain.ApplicationDetailCar(c.Plate, c.Year, c.Make, c.Model)));
                                actualData.Cars = cars;
                            }
                        }
                        else
                        {
                            actualData.Cars = new List<Domain.ApplicationDetailCar>();
                        }

                        if (cmd.Pets.Count > 0)
                        {
                            if (cmd.Pets != null && cmd.Pets.Any())
                            {
                                List<Domain.ApplicationDetailPet> pets = new List<Domain.ApplicationDetailPet>();
                                cmd.Pets.ForEach(p => pets.Add(new Domain.ApplicationDetailPet(p.Type, p.Description, p.Size)));
                                actualData.Pets = pets;
                            }
                        }
                        else
                        {
                            actualData.Pets = new List<Domain.ApplicationDetailPet>();
                        }
                    }
                    await _tenantApplicationDetailsRepository.Update(actualData);
                    var tenantUpdatedEvent = new TenantBasicInformationUpdatedEvent(actualData.Id);
                    var notificationContext1 = _contextFactory.Create(string.Empty, context.CreatedBy);
                    var messageOutboxId1 = await _messageService.SendAsync(notificationContext1, tenantUpdatedEvent);
                }
            }, async ex => {
                var tenantUpdatedEvent = new TenantBasicInformationUpdatedEvent(Guid.Empty);
                var notificationContext1 = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId1 = await _messageService.SendAsync(notificationContext1, tenantUpdatedEvent);
                throw ex;
            });

        }
    }
}
