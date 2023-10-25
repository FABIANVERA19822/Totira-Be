using System;
using System.Collections.Generic;
using System.Text;

namespace Totira.Support.NotificationHub.SignalR
{
    public class IdentityInfo
    {
        public Identity identities { get; set; }

        public class Identity
        {
            public List<string> email { get; set; }
            public string sign_in_provider { get; set; }
        }
    }
}


