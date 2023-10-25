using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class ReferralsCompletionExtension
    {
        public static bool ReferralComplete(this TenantAcquaintanceReferrals referrals)
        {
            return referrals!=null &&
                   referrals.Referrals.Any(x=>!EqualityComparer<TenantAcquaintanceReferral>.Default
                                                    .Equals(x,default(TenantAcquaintanceReferral)));
        }
    }
}
