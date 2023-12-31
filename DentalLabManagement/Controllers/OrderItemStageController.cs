﻿using DentalLabManagement.API.Services.Interfaces;
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
        [HttpPatch(ApiEndPointConstant.OrderItemStage.OrderItemStageEndPoint)]
        public async Task<IActionResult> UpdateOrderItemStage(int id, UpdateOrderItemStageRequest request)
        {
            var isSuccessful = await _orderItemStageService.UpdateOrderItemStage(id, request);
            if (!isSuccessful) return Ok(MessageConstant.OrderItemStage.UpdateStatusStageFailedMessage);
            return Ok(MessageConstant.OrderItemStage.UpdateStatusStageSuccessMessage);
        }

        [HttpPatch(ApiEndPointConstant.OrderItemStage.TransferStageEndPoint)]
        public async Task<IActionResult> TransferStageToAnother(int id, TransferStageRequest request)
        {
            var isSuccessful = await _orderItemStageService.TransferStageToAnother(id, request);
            if (!isSuccessful) return Ok(MessageConstant.OrderItemStage.TransferStageFailedMessage);
            return Ok(MessageConstant.OrderItemStage.TransferStageSuccessMessage);
        }

        [HttpGet(ApiEndPointConstant.OrderItemStage.OrderItemStagesEndPoint)]
        [ProducesResponseType(typeof(OrderItemStageResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderItemStages([FromQuery] OrderItemStageFilter filter, int page, int size)
        {
            var response = await _orderItemStageService.GetOrderItemStages(filter, page, size);
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
