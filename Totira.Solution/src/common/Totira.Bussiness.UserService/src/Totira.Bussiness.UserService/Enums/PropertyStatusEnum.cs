using System.ComponentModel;

namespace Totira.Bussiness.UserService.Enums
{
    public enum PropertyStatusEnum
    {
        [Description("Draft")]
        Draft,

        [Description("A")]
        Published,

        [Description("U")]
        Unpublished
    }
}
