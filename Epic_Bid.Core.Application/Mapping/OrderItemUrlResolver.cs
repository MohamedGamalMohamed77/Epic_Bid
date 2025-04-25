using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Models.Order;
using Epic_Bid.Core.Domain.Entities.Order;
using Epic_Bid.Core.Domain.Entities.Products;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Mapping
{
    class OrderItemUrlResolver<TEntity> : IValueResolver<OrderItem, TEntity, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, TEntity destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureUrl))
                return string.Empty;

            var baseUrl = _configuration.GetSection("Urls")["BaseUrl"];
            var fullUrl = $"{baseUrl}{source.Product.PictureUrl}";

            return fullUrl;
        }
    }
}
