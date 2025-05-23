﻿using Epic_Bid.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Epic_Bid.Infrastructure.Persistence._IdentityAndData.Config.ProductConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.OldPrice)
                 .HasColumnType("decimal(18,2)");
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.ImageUrl).IsRequired();
            builder.Property(p => p.Color).IsRequired();
            builder.Property(p => p.Size).IsRequired();
            builder.Property(p => p.Dimensions).IsRequired();

            // Relations
            builder.HasOne(p => p.ProductCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId);

            // Auction
            builder.Property(p => p.CurrentBid)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(p => p.AuctionBids)
                .WithOne(a => a.Product)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Assuming you want to delete bids when the product is deleted





        }
    }
}
