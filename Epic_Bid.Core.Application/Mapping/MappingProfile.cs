using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Common;
using Epic_Bid.Core.Application.Abstraction.Models.Order;
using Epic_Bid.Core.Application.Abstraction.Models.ProductDt;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Entities.Basket;
using Epic_Bid.Core.Domain.Entities.Order;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Shared;
using Epic_Bid.Shared.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAddress = Epic_Bid.Core.Domain.Entities.Auth.Address;
using OrderAddress = Epic_Bid.Core.Domain.Entities.Order.Address;


namespace Epic_Bid.Core.Application.Mapping
{
    public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			
			CreateMap<ProductCategory, CategoryDto>().ReverseMap();
			CreateMap<Product, ProductDto>()
				.ForMember(des => des.ImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductDto>>());

            CreateMap<Product, ProductByIdDto>()
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ProductCategory.Name))
				.ForMember(des => des.ImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductByIdDto>>());

            CreateMap<CustomerReview, ReviewDto>();
			CreateMap<CreateProductDto, Product>();
			CreateMap<AddReviewDto, CustomerReview>();

			CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
			CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<OrderAddress,AddressDto>().ReverseMap();
			CreateMap<UserAddress, AddressDto>().ReverseMap();

			// Order module
			CreateMap<Order, OrderToReturnDto>()
                 .ForMember(o => o.DeliveryMethod, opt => opt.MapFrom(o => o.DeliveryMethod.ShortName))
                 .ForMember(o => o.DeliveryMethodCost, opt => opt.MapFrom(o => o.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(o => o.ProductId, opt => opt.MapFrom(o => o.Product.ProductId))
                .ForMember(o => o.ProductName, opt => opt.MapFrom(o => o.Product.ProductName))
                .ForMember(o => o.PictureUrl, opt => opt.MapFrom(o => o.Product.PictureUrl))
                .ForMember(o => o.PictureUrl, opt => opt.MapFrom<OrderItemUrlResolver<OrderItemDto>>());
            // Auction
            CreateMap<AuctionBid, AuctionForProductDto>().ReverseMap();

        }

		

	}
}
