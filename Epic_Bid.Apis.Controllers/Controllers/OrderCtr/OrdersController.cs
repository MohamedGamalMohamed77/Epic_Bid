﻿using AutoMapper;
using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Epic_Bid.Core.Application.Abstraction.Common;
using Epic_Bid.Core.Application.Abstraction.Models.Order;
using Epic_Bid.Core.Application.Abstraction.Services.IOrderServ;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Xml;


namespace Epic_Bid.Apis.Controllers.OrderController
{

    public class OrdersController : BaseApiController
    {
        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _OrderService = orderService;
            _Mapper = mapper;
            _UnitOfWork = unitOfWork;
        }

        public IOrderService _OrderService { get; }
        public IMapper _Mapper { get; }
        public IUnitOfWork _UnitOfWork { get; }

        // CreateOrder
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto OrderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var ShippingAddress = _Mapper.Map<AddressDto, Address>(OrderDto.ShippingAddress);
            var Order = await _OrderService.CreateOrderAsync( BuyerEmail!, OrderDto.BasketId, OrderDto.DeliverMethodId, ShippingAddress);
            if(Order is null)
                return BadRequest(new ApiResponse(400, "Problem Creating Order"));
            return Ok(Order);
        }

        // Get Orders For Specific User By BuyerEmail
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("GetOrdersForSpecificUser")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForSpecificUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _OrderService.GetOrdersForSpecificUserAsync(BuyerEmail!);
            if (Orders is null)
                return NotFound(new ApiResponse(404, "There is No Orders For This User"));
            var mappedOrders = _Mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(mappedOrders);
        }

        //Get Order By Id For Specific User
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("GetOrderByIdForSpecificUser")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrderByIdForSpecificUser(int OrderId)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _OrderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, OrderId);
            if (Order is null)
                return NotFound(new ApiResponse(404, "There is No Orders For This User"));
            var mappedOrder = _Mapper.Map<Order, OrderToReturnDto>(Order);
            return Ok(mappedOrder);
        }

        //Get all delivery methods
        [ProducesResponseType(typeof(IReadOnlyList<DeliveryMethod>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("GetAllDeliveryMethods")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethods()
        {
            var DeliveryMethods = await _UnitOfWork.GetRepository<DeliveryMethod>().GetAllAsync();
            if(DeliveryMethods.Count > 0)
                return Ok(DeliveryMethods);
            else
                return NotFound(new ApiResponse(404, "There is No Delivery Methods"));
        }
    }
}
