﻿using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.DataTier.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class TeethPositionController : BaseController<TeethPositionController>
    {
        private readonly ITeethPositionServices _teethPositionServices;

        public TeethPositionController(ILogger<TeethPositionController> logger, ITeethPositionServices teethPositionServices) : base(logger)
        {
            _teethPositionServices = teethPositionServices;
        }

        [HttpPost(ApiEndPointConstant.TeethPosition.TeethPositonsEndPoint)]
        [ProducesResponseType(typeof(TeethPositionResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateTeethPosition(TeethPositionRequest teethPositionRequest)
        {
            var response = await _teethPositionServices.CreateTeethPosition(teethPositionRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.TeethPosition.TeethPositonsEndPoint)]
        [ProducesResponseType(typeof(IPaginate<TeethPositionResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetTeethPositons([FromQuery] TeethPositionFilter filter, int page, int size)
        {
            var response = await _teethPositionServices.GetTeethPositions(filter, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.TeethPosition.TeethPositonEndPoint)]
        [ProducesResponseType(typeof(IPaginate<TeethPositionResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetTeethPositons(int id)
        {
            var response = await _teethPositionServices.GetTeethPositionById(id);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.TeethPosition.TeethPositonEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateTeethPosition(int id, UpdateTeethPositionRequest request)
        {
            var isSuccessful = await _teethPositionServices.UpdateTeethPosition(id, request);
            if (!isSuccessful) return Ok(MessageConstant.TeethPosition.UpdateFailedMessage);
            return Ok(MessageConstant.TeethPosition.UpdateSucessMessage);
        }
    }
}
