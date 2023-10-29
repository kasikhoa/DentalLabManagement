using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.OrderItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{   
    [ApiController]
    public class OrderItemController : BaseController<OrderItemController>
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(ILogger<OrderItemController> logger, IOrderItemService orderItemService) : base(logger)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet(ApiEndPointConstant.OrderItem.OrderItemsEndPoint)]
        [ProducesResponseType(typeof(GetOrderItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderItems(int? orderId, string? warrantyCardCode, int page, int size)
        {
            var response = await _orderItemService.GetOrderItems(orderId, warrantyCardCode, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.OrderItem.OrderItemEndPoint)]
        [ProducesResponseType(typeof(GetOrderItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderItemById(int id)
        {
            var response = await _orderItemService.GetOrderItemById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.OrderItem.OrderItemEndPoint)]
        public async Task<IActionResult> UpdateOrderItem(int id, UpdateOrderItemRequest request)
        {
            var isSuccessful = await _orderItemService.UpdateOrderItem(id, request);
            if (!isSuccessful) return Ok(MessageConstant.OrderItem.UpdateFailedMessage);
            return Ok(MessageConstant.OrderItem.UpdateSuccessMessage);
        }

        [HttpPut(ApiEndPointConstant.OrderItem.OrderItemCardEndPoint)]
        [ProducesResponseType(typeof(GetOrderItemResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertWarrantyCard(int id, InsertWarrantyCardRequest updateRequest)
        {
            var response = await _orderItemService.InsertWarrantyCard(id, updateRequest);
            return Ok(response);
        }

    }
}
