namespace Totira.Bussiness.PropertiesService.Enums
{
    using System.ComponentModel;

    public enum ParametersSearchProperty
    {
        [Description("Ml_num")]
        Id = 0,

        [Description("Lp_dol")]
        List_price = 1,

        [Description("S_r")]
        S_R = 2,

        [Description("StandardNames")]
        StandardNames = 0,

        [Description("Limit")]
        Limit = 30000
    }
}
