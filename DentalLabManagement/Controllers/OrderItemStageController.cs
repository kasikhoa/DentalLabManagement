using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class OrderItemStageController : BaseController<OrderItemStageController>
    {
        private readonly IOrderItemStageService _orderItemStageService;

        public OrderItemStageController(ILogger<OrderItemStageController> logger, IOrderItemStageService orderItemStageService) : base(logger)
        {
            _orderItemStageService = orderItemStageService;
        }

        [HttpPut(ApiEndPointConstant.OrderItemStage.OrderItemStageEndPoint)]
        [ProducesResponseType(typeof(UpdateOrderItemStageResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrderItemStage(int id, UpdateOrderItemStageRequest updateOrderItemStageRequest)
        {
            var response = await _orderItemStageService.UpdateOrderItemStage(id, updateOrderItemStageRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.OrderItemStage.OrderItemStagesEndPoint)]
        [ProducesResponseType(typeof(UpdateOrderItemStageResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderItemStages(int? orderItemId, int? staffId, int? indexStage, OrderItemStageStatus? status, int page, int size)
        {
            var response = await _orderItemStageService.GetOrderItemStages(orderItemId, staffId, indexStage, status, page, size);
            return Ok(response);
        }
    }
}
