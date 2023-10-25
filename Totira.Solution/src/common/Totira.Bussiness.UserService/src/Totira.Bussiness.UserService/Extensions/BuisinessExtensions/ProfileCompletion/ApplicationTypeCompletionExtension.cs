using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class ApplicationTypeCompletionExtension
    {
        public static bool ApplicationTypeComplete(this TenantApplicationType application)
        {
            return application != null &&
                   !string.IsNullOrWhiteSpace(application.ApplicationType);
        }
    }
}
