using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Models.Auth
{
	public class ForgetPasswordDto
	{
		[Required(ErrorMessage ="Email is required please ..!")]
		[EmailAddress]
        public required string Email { get; set; }

    }
}
