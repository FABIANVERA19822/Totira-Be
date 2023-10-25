

using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryTenantGroupShareProfileForCheckCodeAndEmail: IQuery
    {
        public Guid TenantId { get; set; }
        public int AccessCode { get; set; }
        public string Email { get; set; }
        public QueryTenantGroupShareProfileForCheckCodeAndEmail(Guid tenantId, int accessCode, string Email)
        {
            this.TenantId = tenantId;
            this.AccessCode = accessCode;
            this.Email = Email;
        }
    }
}
