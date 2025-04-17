using Epic_Bid.Core.Application.Abstraction.Services;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Services
{
	public class ServiceManager : IServiceManager
	{

		private readonly Lazy<IAuthService> _authservice;

		public ServiceManager(Func<IAuthService> authServiceFactory)
		{
			_authservice = new Lazy<IAuthService>(authServiceFactory,LazyThreadSafetyMode.ExecutionAndPublication); 
		}


		public IAuthService AuthService => _authservice.Value;

	}
}
