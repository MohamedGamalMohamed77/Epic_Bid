using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services
{
	public interface IServiceManager
	{
		public IAuthService AuthService { get; }
	}
}
