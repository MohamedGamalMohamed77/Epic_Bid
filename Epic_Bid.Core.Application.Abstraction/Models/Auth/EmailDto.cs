using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Models.Auth
{
	public class EmailDto
	{
		public required string To {get; set;}
		public required string Subject {get; set;}
		public required string Body {get; set;}
	}
		
}
