﻿using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Payload.Payment;
using DentalLabManagement.BusinessTier.Payload.OrderHistory;
using DentalLabManagement.BusinessTier.Validators;
using Microsoft.AspNetCore.Authorization;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class OrderController : BaseController<OrderController>
    {
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService) : base(logger)
        {
            _orderService = orderService;
        }

        [CustomAuthorize(RoleEnum.Reception, RoleEnum.Partner)]
        [HttpPost(ApiEndPointConstant.Order.OrdersEndPoint)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewOrder(CreateOrderRequest order)
        {
            var response = await _orderService.CreateNewOrder(order);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Order.OrderEndPoint)]
        [ProducesResponseType(typeof(GetOrderDetailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var response = await _orderService.GetOrderTeethDetail(id);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Order.OrdersEndPoint)]
        [ProducesResponseType(typeof(GetOrdersResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders([FromQuery] OrderFilter filter, int page, int size)
        {
            var response = await _orderService.GetOrders(filter, page, size);
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.Reception)]
        [HttpPatch(ApiEndPointConstant.Order.OrderEndPoint)]
        public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderRequest updateOrderRequest)
        {
            var isSuccessful = await _orderService.UpdateOrderStatus(id, updateOrderRequest);
            if (!isSuccessful) return Ok(MessageConstant.Order.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Order.UpdateStatusSuccessMessage);
        }

        [HttpPost(ApiEndPointConstant.Order.OrderPaymentsEndPoint)]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrderPayment(int id, PaymentRequest paymentRequest)
        {
            var response = await _orderService.UpdateOrderPayment(id, paymentRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Order.OrderPaymentsEndPoint)]
        [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderPayments(int id, [FromQuery] PaymentFilter filter, int page, int size)
        {
            var response = await _orderService.GetOrderPayments(id, filter, page, size);
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.Partner, RoleEnum.Reception)]
        [HttpPost(ApiEndPointConstant.Order.WarrantyRequestsEndPoint)]
        [ProducesResponseType(typeof(OrderHistoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWarrantyRequest(int id, CreateWarrantyRequest request)
        {
            var response = await _orderService.CreateWarrantyRequest(id, request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Order.OrderHistoryEndPoint)]
        [ProducesResponseType(typeof(OrderHistoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewOrderHistory(int id, [FromQuery] OrderHistoryFilter filter, int page, int size)
        {
            var response = await _orderService.ViewOrderHistory(id, filter, page, size);
            return Ok(response);
        }

    }
}
