using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class BasicInfoCompletionExtension
    {
        public static bool BasicComplete(this TenantBasicInformation basic)
        {

            return basic != null &&
                   !string.IsNullOrWhiteSpace(basic.FirstName) &&
                   !string.IsNullOrWhiteSpace(basic.LastName) &&
                   basic.Birthday != null &&
                   basic.Birthday.Year != 0 &&
                   basic.Birthday.Month != 0 &&
                   basic.Birthday.Day != 0;
        }
    }
}
