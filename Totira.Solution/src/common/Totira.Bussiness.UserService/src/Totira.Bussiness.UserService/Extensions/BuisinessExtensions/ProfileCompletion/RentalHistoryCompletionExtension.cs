using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class RentalHistoryCompletionExtension
    {
        public static bool HistoryComplete(this TenantRentalHistories histories)
        {
            return histories != null &&
                   histories.RentalHistories.Any(x => !EqualityComparer<TenantRentalHistory>.Default
                                                    .Equals(x, default(TenantRentalHistory)));
        }
    }
}
