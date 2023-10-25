using System;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
	public class QueryTenantShareProfileByTenantId : IQuery
    {
        public Guid TenantId { get; }
        public string EncryptedAccessCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public QueryTenantShareProfileByTenantId(Guid tenantId, string encryptedAccessCode, string email)
        {
            TenantId = tenantId;
            EncryptedAccessCode = encryptedAccessCode;
            Email = email;
        }
    }
}

