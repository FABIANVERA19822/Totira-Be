using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Bussiness.UserService.Enums
{
    public enum PropertyClaimStatusEnum
    {
        [Description("Pending")]
        Pending,

        [Description("Approved")]
        Approved,

        [Description("Rejected")]
        Rejected
    }

}
