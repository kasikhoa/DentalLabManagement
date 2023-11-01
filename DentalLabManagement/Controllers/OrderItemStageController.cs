using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using DentalLabManagement.BusinessTier.Validators;
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

        [CustomAuthorize(RoleEnum.Staff, RoleEnum.Reception)]
        [HttpPut(ApiEndPointConstant.OrderItemStage.OrderItemStageEndPoint)]
        public async Task<IActionResult> UpdateOrderItemStage(int id, UpdateOrderItemStageRequest request)
        {
            var isSuccessful = await _orderItemStageService.UpdateOrderItemStage(id, request);
            if (!isSuccessful) return Ok(MessageConstant.OrderItemStage.UpdateStatusStageFailedMessage);
            return Ok(MessageConstant.OrderItemStage.UpdateStatusStageSuccessMessage);
        }

        [HttpGet(ApiEndPointConstant.OrderItemStage.OrderItemStagesEndPoint)]
        [ProducesResponseType(typeof(OrderItemStageResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderItemStages(int? orderItemId, int? staffId, int? indexStage, OrderItemStageStatus? status, 
            OrderItemStageMode? mode, int page, int size)
        {
            var response = await _orderItemStageService.GetOrderItemStages(orderItemId, staffId, indexStage, status, mode, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.OrderItemStage.OrderItemStageEndPoint)]
        [ProducesResponseType(typeof(OrderItemStageResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderItemStageById(int id)
        {
            var response = await _orderItemStageService.GetOrderItemStageById(id);
            return Ok(response);
        }
    }
}
