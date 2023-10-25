using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Bussiness.UserService.Enums
{
    public enum TenantApplicationRolesEnum
    {
        [Description("Main tenant")]
        Main,

        [Description("Co-applicant")]
        Coapplicant,

        [Description("Guarantor")]
        Guarantor
    }
}
