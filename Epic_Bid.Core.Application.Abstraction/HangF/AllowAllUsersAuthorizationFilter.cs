using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
namespace Epic_Bid.Shared.HangFire
{
    public class AllowAllUsersAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
    {
        public bool Authorize(Hangfire.Dashboard.DashboardContext context)
        {
            return true; // يسمح لأي حد يفتح الداشبورد
        }
    }
}
