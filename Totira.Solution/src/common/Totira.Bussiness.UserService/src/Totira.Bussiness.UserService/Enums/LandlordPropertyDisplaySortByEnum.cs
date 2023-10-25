using System.ComponentModel;

namespace Totira.Bussiness.UserService.Enums
{
    public enum LandlordPropertyDisplaySortByEnum
    {
        [Description("Size")]
        Size = 1,

        [Description("Bedrooms")]
        Bedrooms = 2,

        [Description("Bathrooms")]
        Bathrooms = 3,

        [Description("Price")]
        Price = 4,

        [Description("AvailableDate")]
        AvailableDate = 5,

        [Description("ApplicationCount")]
        ApplicationCount = 0
    }
}
