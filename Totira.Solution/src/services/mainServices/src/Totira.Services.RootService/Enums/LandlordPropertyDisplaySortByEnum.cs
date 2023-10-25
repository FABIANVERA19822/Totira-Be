using System.ComponentModel;

namespace Totira.Services.RootService.Enums
{
    public enum LandlordPropertyDisplaySortByEnum
    {
        [Description("Size")]
        Size,

        [Description("Bedrooms")]
        Bedrooms,

        [Description("Bathrooms")]
        Bathrooms,

        [Description("Price")]
        Price,

        [Description("AvailableDate")]
        AvailableDate,

        [Description("ApplicationCount")]
        ApplicationCount
    }
}
