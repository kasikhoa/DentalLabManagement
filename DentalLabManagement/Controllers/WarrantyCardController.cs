using DentalLabManagement.API.Services.Implements;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.WarrantyCard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class WarrantyCardController : BaseController<WarrantyCardController>
    {
        private readonly IWarrantyCardService _warrantyCardService;

        public WarrantyCardController(ILogger<WarrantyCardController> logger, IWarrantyCardService warrantyCardService) : base(logger)
        {
            _warrantyCardService = warrantyCardService;
        }

        [HttpPost(ApiEndPointConstant.WarrantyCard.WarrantyCardsEndPoint)]
        [ProducesResponseType(typeof(CreateWarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWarrantyCard(CreateWarrantyCardRequest request)
        {
            var response = await _warrantyCardService.CreateNewWarrantyCard(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.WarrantyCard.WarrantyCardsEndPoint)]
        [ProducesResponseType(typeof(WarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarrantyCards(string? cardCode, int? cardTypeId, WarrantyCardStatus? status, int page, int size)
        {
            var response = await _warrantyCardService.GetWarrantyCards(cardCode, cardTypeId, status, page, size);
            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.WarrantyCard.WarrantyCardEndPoint)]
        public async Task<IActionResult> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request)
        {
            var isSuccessful = await _warrantyCardService.UpdateWarrantyCard(id, request);
            if (!isSuccessful) return Ok(MessageConstant.WarrantyCard.UpdateFailedMessage);
            return Ok(MessageConstant.WarrantyCard.UpdatedSuccessMessage);
        }

        [HttpGet(ApiEndPointConstant.WarrantyCard.WarrantyCardEndPoint)]
        [ProducesResponseType(typeof(WarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarrantyCardById(int id)
        {
            var response = await _warrantyCardService.GetWarrantyCardById(id);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.WarrantyCard.WarrantyCardEndPointv2)]
        [ProducesResponseType(typeof(WarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarrantyCardByCode(string cardCode, string cardTypeCode)
        {
            var response = await _warrantyCardService.GetWarrantyCardByCode(cardCode, cardTypeCode);
            return Ok(response);
        }
    }
}
