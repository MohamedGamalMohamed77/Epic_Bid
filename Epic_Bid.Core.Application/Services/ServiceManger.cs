using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Services;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Basket;
using Epic_Bid.Core.Application.Abstraction.Services.IProductServ;
using Epic_Bid.Core.Application.Services.ProductServ;
using Epic_Bid.Core.Domain.Contracts.Persistence;
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
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, Func<IAuthService> authServiceFactory,Func<IBasketService> basketServiceFactory)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

			_authService = new Lazy<IAuthService>(authServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
			_productService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
			_basketService = new Lazy<IBasketService>(basketServiceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		#region Auth
		private readonly Lazy<IAuthService> _authService;
		public IAuthService AuthService => _authService.Value;
		#endregion

		#region ProductService

		private readonly Lazy<IProductService> _productService;
		public IProductService ProductService => _productService.Value;

		#endregion

		#region BasketService
		private readonly Lazy<IBasketService> _basketService;
		public IBasketService BasketService => _basketService.Value; 
		#endregion
	}
}
