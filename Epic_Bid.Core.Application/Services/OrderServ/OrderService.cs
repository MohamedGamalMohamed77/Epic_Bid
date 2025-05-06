using Epic_Bid.Core.Application.Abstraction.Services.IOrderServ;
using Epic_Bid.Shared.Exceptions;
using Epic_Bid.Core.Application.SpecificationImplementation.OrderSpec;
using Epic_Bid.Core.Domain.Contracts.Infrastructure;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Order;
using Epic_Bid.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;


namespace Epic_Bid.Core.Application.Services.OrderServ
{
    public class OrderService : IOrderService
    {
        public OrderService(IBasketRepository BasketRepo, IUnitOfWork UnitOfWork)
        {
            this._BasketRepo = BasketRepo;
            this._UnitOfWork = UnitOfWork;
        }

        public IBasketRepository _BasketRepo { get; }
        public IUnitOfWork _UnitOfWork { get; }

        #region CreateOrderAsync
        public async Task<Order> CreateOrderAsync(string BuyerEmail, string BasketId, int DelvieryMethodId, Address ShippingAddress)
        {
            //Get Selected Item From Basket Repo, Ceating the OrderItem
            var Basket = await _BasketRepo.GetAsync(BasketId);
            if (Basket is null)
            {
                throw new NotFoundException(nameof(Basket), BasketId);
            }
            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count() > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _UnitOfWork.GetRepository<Product>().GetByIdAsync(item.Id);
                    if (product is null)
                    {
                        throw new NotFoundException(nameof(product), item.Id);
                    }
                    var ProductItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.ImageUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, product.Price);
                    OrderItems.Add(OrderItem);
                }
            }
            if (OrderItems.Count() == 0)
            {
                throw new NotFoundException(nameof(OrderItems), BasketId);
            }
            //3 Calculate SubTotal
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _UnitOfWork.GetRepository<DeliveryMethod>().GetByIdAsync(DelvieryMethodId);
            if (DeliveryMethod is null)
            {
                throw new NotFoundException(nameof(DeliveryMethod), DelvieryMethodId);
            }
            //5.Create Order

            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal, Basket?.PaymentIntentId!);

            try
            {
                //6.Add Order Locally
                await _UnitOfWork.GetRepository<Order>().AddAsync(Order);
                //7.Save Order To DateBased[ToD]
                await _UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.Message);
            }
            return Order;

        }

        #endregion

        #region GetOrderByIdForSpecificUserAsync
        public Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpecification(buyerEmail, OrderId);
            var Order = _UnitOfWork.GetRepository<Order>().GetByIdAsync(spec);
            if (Order is null)
            {
                throw new NotFoundException(nameof(Order), OrderId);
            }
            return Order!;

        }

        #endregion

        #region GetOrdersForSpecificUserAsync
        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var Orders = await _UnitOfWork.GetRepository<Order>().GetAllAsync(spec);
            if (Orders is null)
            {
                throw new NotFoundException(nameof(Orders), buyerEmail);
            }
            return Orders;
        }

        #endregion
    }
}
