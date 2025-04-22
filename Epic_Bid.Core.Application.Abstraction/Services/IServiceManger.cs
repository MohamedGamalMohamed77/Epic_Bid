using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.IProductServ;
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
        public IProductService ProductService { get;  }
    }
}
