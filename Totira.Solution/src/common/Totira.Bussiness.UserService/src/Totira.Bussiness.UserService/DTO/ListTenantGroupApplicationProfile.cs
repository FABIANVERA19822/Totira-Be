
using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.DTO
{
    public class ListTenantGroupApplicationProfile
    {
        public int Count { get; set; } = 0;
        public List<TenantGroupApplicationProfile> GroupApplicationProfiles { get; set; } = new List<TenantGroupApplicationProfile>();
    }
}
