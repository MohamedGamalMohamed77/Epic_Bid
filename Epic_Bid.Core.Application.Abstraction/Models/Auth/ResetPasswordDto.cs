using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Models.Auth
{
	public class ResetPasswordDto
	{
		public required string Email { get; set; }
		public required string NewPassword { get; set; }

	}
}
