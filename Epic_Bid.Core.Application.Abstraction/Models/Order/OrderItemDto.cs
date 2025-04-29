﻿
namespace Epic_Bid.Core.Application.Abstraction.Models.Order
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}