using System.ComponentModel;
namespace Totira.Bussiness.UserService.Enums
{
    public enum TenantPropertyApplicationStatusEnum
    {
        [Description("Approved")]
        Approved,

        [Description("UnderRevision")]
        UnderRevision,

        [Description("Rejected")]
        Rejected,
        
        [Description("Canceled")]
        Canceled,
    }
}
