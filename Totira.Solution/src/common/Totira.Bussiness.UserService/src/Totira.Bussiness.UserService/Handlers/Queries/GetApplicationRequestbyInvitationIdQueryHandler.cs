using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetApplicationRequestbyInvitationIdQueryHandler : IQueryHandler<QueryApplicationRequestbyInvitationId, GetApplicationRequestbyInvitationDto>
    {
        private readonly ILogger<GetApplicationRequestbyInvitationIdQueryHandler> _logger;
        private readonly IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> _tenantApplicationRequestCoapplicantsSendEmailsRepository;

        public GetApplicationRequestbyInvitationIdQueryHandler(
           IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> tenantApplicationRequestCoapplicantsSendEmailsRepository,
            ILogger<GetApplicationRequestbyInvitationIdQueryHandler> logger
            )
        {
            _tenantApplicationRequestCoapplicantsSendEmailsRepository = tenantApplicationRequestCoapplicantsSendEmailsRepository;
            _logger = logger;
        }

        public async Task<GetApplicationRequestbyInvitationDto> HandleAsync(QueryApplicationRequestbyInvitationId query)
        {
            var info = await _tenantApplicationRequestCoapplicantsSendEmailsRepository.GetByIdAsync(query.InvitationId);
            var result =
               info != null ?
                new GetApplicationRequestbyInvitationDto() {Id = info.Id, ApplicationRequestId= info.ApplicationRequestId, CoapplicantEmail = info.CoapplicantEmail, IsActived= info.dateTimeExpiration > DateTime.Now } :
                new GetApplicationRequestbyInvitationDto();

            return result;
        }
    }
}
