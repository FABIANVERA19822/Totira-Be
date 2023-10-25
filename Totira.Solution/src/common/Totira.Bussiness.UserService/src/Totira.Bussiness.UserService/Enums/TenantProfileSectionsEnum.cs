using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Bussiness.UserService.Enums
{
    public enum TenantProfileSectionsEnum
    {
        [Description("Application Details")]
        Application,

        [Description("Basic Info")]
        Basic,

        [Description("Contact Info")]
        Contact,

        [Description("Employment & Income")]
        Employment,

        [Description("Upload Official ID")]
        OfficialID,

        [Description("Rental History")]
        RentalHistory,

        [Description("Other Referrals")]
        Referrals,

        [Description("Upload Picture")]
        ImageProfile,

        [Description("Application Type")]
        ApplicationType,

        [Description("Cosigners Info")]
        Cosigners
    }
}
