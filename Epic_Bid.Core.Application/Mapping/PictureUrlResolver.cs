using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Models.ProductDt;
using Epic_Bid.Core.Domain.Entities.Products;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Mapping
{

    public class PictureUrlResolver<TEntity> : IValueResolver<Product, TEntity, string>
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, TEntity destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ImageUrl))
                return string.Empty;

            var baseUrl = _configuration.GetSection("Urls")["BaseUrl"];
            var fullUrl = $"{baseUrl}{source.ImageUrl}";

            return fullUrl;
        }
    }       
}
