using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Common;
using Epic_Bid.Core.Domain.Entities;
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
		}


	}
}
