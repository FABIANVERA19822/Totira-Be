using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantApplicationDetailsCommandHandler : IMessageHandler<UpdateTenantApplicationDetailsCommand>
    {
        private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;
        private readonly ILogger<UpdateTenantBasicInformationCommandHandler> _logger;

        public UpdateTenantApplicationDetailsCommandHandler(
            IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
            ILogger<UpdateTenantBasicInformationCommandHandler> logger
            )
        {
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, UpdateTenantApplicationDetailsCommand command)
        {
            _logger.LogDebug($"updating tenant personal information with id {command.Id}");


            Expression<Func<TenantApplicationDetails, bool>> expression = (ad => ad.TenantId == command.Id);

            var response = await _tenantApplicationDetailsRepository.Get(expression);

            var actualData = response.MaxBy(x => x.CreatedOn);

            if (actualData != null)
            {
                if (command.IsVerificationsRequested.HasValue && command.IsVerificationsRequested.Value)
                {
                    actualData.IsVerificationsRequested = command.IsVerificationsRequested.Value;
                    await _tenantApplicationDetailsRepository.Update(actualData);
                    var tenantVerificationUpdatedEvent = new TenantBasicInformationUpdatedEvent(actualData.Id);
                    return;
                }
                else
                {
                    if (command.EstimatedMove != null)
                    {
                        var estimatedMove = new Domain.ApplicationDetailEstimatedMove(command.EstimatedMove.Month, command.EstimatedMove.Year);
                        actualData.EstimatedMove = estimatedMove;
                    }

                    if (actualData.Smoker != command.Smoker)
                    {
                        actualData.Smoker = command.Smoker;
                    }

                    if (actualData.EstimatedRent != command.EstimatedRent && command.EstimatedRent != string.Empty)
                    {
                        actualData.EstimatedRent = command.EstimatedRent;
                    }

                    if (actualData.Occupants.Adults != command.Occupants.Adults)
                    {
                        actualData.Occupants.Adults = command.Occupants.Adults;
                    }

                    if (actualData.Occupants.Childrens != command.Occupants.Children)
                    {
                        actualData.Occupants.Childrens = command.Occupants.Children;
                    }

                    if (command.Cars.Count > 0)
                    {
                        if (command.Cars != null && command.Cars.Any())
                        {
                            List<Domain.ApplicationDetailCar> cars = new List<Domain.ApplicationDetailCar>();
                            command.Cars.ForEach(c => cars.Add(new Domain.ApplicationDetailCar(c.Plate, c.Year, c.Make, c.Model)));
                            actualData.Cars = cars;
                        }
                    } else
                    {
                        actualData.Cars = new List<Domain.ApplicationDetailCar>();
                    }

                    if (command.Pets.Count > 0)
                    {
                        if (command.Pets != null && command.Pets.Any())
                        {
                            List<Domain.ApplicationDetailPet> pets = new List<Domain.ApplicationDetailPet>();
                            command.Pets.ForEach(p => pets.Add(new Domain.ApplicationDetailPet(p.Type, p.Description, p.Size)));
                            actualData.Pets = pets;
                        }
                    } else
                    {
                        actualData.Pets = new List<Domain.ApplicationDetailPet>();
                    }
                }
                await _tenantApplicationDetailsRepository.Update(actualData);
                var tenantUpdatedEvent = new TenantBasicInformationUpdatedEvent(actualData.Id);

            }


        }
    }
}
