
namespace Totira.Bussiness.UserService.Handlers.Queries
{
    using Microsoft.Extensions.Logging;
    using static Totira.Support.Persistance.IRepository;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Queries;
    using Totira.Support.Application.Queries;
    using Totira.Bussiness.UserService.DTO;
    using System.Linq.Expressions;

    public class GetAllInvitationbyApplicationRequestIdQueryHandler : IQueryHandler<QueryInvitationsByApplicationRequestById, GetAllInvitationsToJoinByApplicationRequestDto>
    {
        private readonly ILogger<GetAllInvitationbyApplicationRequestIdQueryHandler> _logger;
        private readonly IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> _tenantApplicationRequestCoapplicantsSendEmailsRepository;

        public GetAllInvitationbyApplicationRequestIdQueryHandler(
           IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> tenantApplicationRequestCoapplicantsSendEmailsRepository,
            ILogger<GetAllInvitationbyApplicationRequestIdQueryHandler> logger
            )
        {
            _tenantApplicationRequestCoapplicantsSendEmailsRepository = tenantApplicationRequestCoapplicantsSendEmailsRepository;
            _logger = logger;
        }

        public async Task<GetAllInvitationsToJoinByApplicationRequestDto> HandleAsync(QueryInvitationsByApplicationRequestById query)
        {
            Expression<Func<TenantApplicationRequestCoapplicantsSendEmails, bool>> expression = (tcj => tcj.ApplicationRequestId == query.ApplicationRequestId);
            var invitations = await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Get(expression);

            var invitationsDto = new List<GetAllInvitationstoJoin>();
 
           foreach (var invitation in invitations)
            {
               var item= new GetAllInvitationstoJoin()
                {
                   Id = invitation.Id, 
                   CoapplicantEmail = invitation.CoapplicantEmail,
                   UpdateOn = invitation.UpdatedOn,
                   IsActived = invitation.dateTimeExpiration > DateTime.Now
                };

                invitationsDto.Add(item);
            } 

            var result = new GetAllInvitationsToJoinByApplicationRequestDto()
            {
               ApplicationRequestId = query.ApplicationRequestId,
               Invitations = invitationsDto
            };
             
            return result;
        }
    }
}
