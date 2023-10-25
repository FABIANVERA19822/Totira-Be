using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryTenantShareProfileForCheckCodeAndEmail : IQuery
{
    public Guid TenantId { get; set; }
    public int AccessCode { get; set; }
    public string Email { get; set; }
    public QueryTenantShareProfileForCheckCodeAndEmail(Guid tenantId, int accessCode, string Email)
    {
        this.TenantId = tenantId;
        this.AccessCode = accessCode;
        this.Email = Email;
    }

}
