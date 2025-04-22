using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Common;
using Epic_Bid.Core.Application.Abstraction.Models.ProductDt;
using Epic_Bid.Core.Domain.Entities;
using Epic_Bid.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Address,AddressDto>().ReverseMap();
			CreateMap<ProductCategory, CategoryDto>().ReverseMap();
			CreateMap<Product, ProductDto>()
				.ForMember(des => des.ImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductDto>>());

            CreateMap<Product, ProductByIdDto>()
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ProductCategory.Name))
				.ForMember(des => des.ImageUrl, opt => opt.MapFrom<PictureUrlResolver<ProductByIdDto>>());

            CreateMap<CustomerReview, ReviewDto>();
			CreateMap<CreateProductDto, Product>();
			CreateMap<AddReviewDto, CustomerReview>();
        }

		

	}
}
