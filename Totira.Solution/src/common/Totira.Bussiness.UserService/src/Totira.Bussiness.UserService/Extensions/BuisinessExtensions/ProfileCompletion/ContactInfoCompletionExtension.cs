using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class ContactInfoCompletionExtension
    {
        public static bool ContactComplete(this TenantContactInformation contact)
        {
            return contact != null &&
                   !string.IsNullOrWhiteSpace(contact.HousingStatus) &&
                   !string.IsNullOrWhiteSpace(contact.Country) &&
                   !string.IsNullOrWhiteSpace(contact.Province) &&
                   !string.IsNullOrWhiteSpace(contact.City) &&
                   !string.IsNullOrWhiteSpace(contact.ZipCode) &&
                   !string.IsNullOrWhiteSpace(contact.StreetAddress) &&
                   !string.IsNullOrWhiteSpace(contact.Email) &&
                   !string.IsNullOrWhiteSpace(contact.PhoneNumber.CountryCode) &&
                   !string.IsNullOrWhiteSpace(contact.PhoneNumber.Number);
        }
    }
}
