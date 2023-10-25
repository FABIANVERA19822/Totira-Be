using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class ProfileImageCompletionExtension
    {
        public static bool ProfileImageComplete(this GetTenantProfileImageDto image)
        {
            return image is not null &&
                   !string.IsNullOrWhiteSpace(image.Filename.FileName) &&
                   !string.IsNullOrWhiteSpace(image.Filename.ContentType);
        }
    }
}
