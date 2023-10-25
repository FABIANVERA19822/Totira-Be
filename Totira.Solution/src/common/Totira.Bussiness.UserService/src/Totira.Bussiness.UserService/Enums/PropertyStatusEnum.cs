using System.ComponentModel;

namespace Totira.Bussiness.UserService.Enums
{
    public enum PropertyStatusEnum
    {
        [Description("Draft")]
        Draft,

        [Description("Published")]
        Published,

        [Description("Unpublished")]
        Unpublished
    }
}
