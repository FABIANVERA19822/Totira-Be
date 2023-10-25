using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
	public class DeleteTenantUnacceptedApplicationRequestCommandHandler : IMessageHandler<DeleteTenantUnacceptedApplicationRequestCommand>
    {
        private readonly ILogger<DeleteTenantUnacceptedApplicationRequestCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _requestRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _basicInfoRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public DeleteTenantUnacceptedApplicationRequestCommandHandler(
            IRepository<TenantApplicationRequest, Guid> requestRepository,
            IRepository<TenantBasicInformation, Guid> basicInformationRepository,
            ILogger<DeleteTenantUnacceptedApplicationRequestCommandHandler> logger,
            IContextFactory contextFactory,
             IMessageService messageService)
        {
            _requestRepository = requestRepository;
            _basicInfoRepository = basicInformationRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, DeleteTenantUnacceptedApplicationRequestCommand command)
        {
            var basicInfo = (await _basicInfoRepository.Get(x => x.TenantEmail == command.Email)).FirstOrDefault();

            Expression<Func<TenantApplicationRequest, bool>> requestExpression = r =>
                              r.Id != command.RequestId &&
                              (
                                r.Coapplicants.Any(ca => ca.Email == command.Email) ||
                                r.Guarantor.Email == command.Email ||
                                r.TenantId == basicInfo.Id
                              );

            var requests = await _requestRepository.Get(requestExpression);

            foreach (var request in requests)
            {
                if (request.Guarantor?.Email == basicInfo.TenantEmail)
                {
                    request.Guarantor = null;
                    await _requestRepository.Update(request);
                }

                if (request.Coapplicants is not null && request.Coapplicants.Any(x => x.Email == basicInfo.TenantEmail))
                {
                    var coapplicant = request.Coapplicants.Where(x => x.Email == basicInfo.TenantEmail).FirstOrDefault();
                    request.Coapplicants.Remove(coapplicant);
                    await _requestRepository.Update(request);
                }

                if (request.TenantId == basicInfo.Id)
                {
                    await _requestRepository.Delete(request);
                }
            }

            var userCreatedEvent = new TenantUnacceptedApplicationRequestDeletedCommandEvent(basicInfo.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);

        }
    }
}

