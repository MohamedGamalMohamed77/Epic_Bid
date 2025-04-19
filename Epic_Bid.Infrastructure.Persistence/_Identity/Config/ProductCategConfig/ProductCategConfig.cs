using Epic_Bid.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Epic_Bid.Infrastructure.Persistence._Identity.Config.ProductCategConfig
{
    public  class ProductCategConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public  void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(p => p.Name).IsRequired();
        }
    }
}
