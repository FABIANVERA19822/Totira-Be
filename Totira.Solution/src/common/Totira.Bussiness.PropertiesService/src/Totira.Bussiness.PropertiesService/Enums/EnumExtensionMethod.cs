using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Bussiness.PropertiesService.Enums
{
    public static class EnumExtensionMethod
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo != null)
            {
                var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
            }
            return enumValue.ToString();
        }
    }
}
