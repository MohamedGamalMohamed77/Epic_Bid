using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Domain.Entities
{
	public class ApplicationUser : IdentityUser
	{
        public required string DisplayName { get; set; }
		public int ResetCode { get; set; }
		public DateTime ResetCodeExpire { get; set; }
		public virtual Address? Address { get; set; }
    }
}
