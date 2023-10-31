using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Payload.Payment;
using DentalLabManagement.BusinessTier.Payload.OrderHistory;

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

        [HttpPost(ApiEndPointConstant.Order.OrdersEndPoint)]
        [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewOrder (CreateOrderRequest order)
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
        public async Task<IActionResult> GetOrders(string? invoiceId, string? dentalName, OrderStatus? status, 
            OrderPaymentStatus? paymentStatus, int page, int size)
        {
            var response = await _orderService.GetOrders(invoiceId, dentalName, status, paymentStatus, page, size);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.Order.OrderEndPoint)]
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
        public async Task<IActionResult> GetOrderPayments(int id, PaymentType? type, PaymentStatus? status, 
            int page, int size)
        {
            var response = await _orderService.GetOrderPayments(id, type, status, page, size);
            return Ok(response);
        }

        [HttpPost(ApiEndPointConstant.Order.WarrantyRequestsEndPoint)]
        [ProducesResponseType(typeof(WarrantyResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWarrantyRequest(int id, CreateWarrantyRequest request)
        {
            var response = await _orderService.CreateWarrantyRequest(id, request);
            return Ok(response);
        }
    }
}
