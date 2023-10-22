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
        [ProducesResponseType(typeof(WarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWarrantyCard(WarrantyCardRequest request)
        {
            var response = await _warrantyCardService.InseartNewWarrantyCard(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.WarrantyCard.WarrantyCardsEndPoint)]
        [ProducesResponseType(typeof(WarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarrantyCards(string? cardCode, int? categoryId, WarrantyCardStatus? status, int page, int size)
        {
            var response = await _warrantyCardService.GetWarrantyCards(cardCode, categoryId, status, page, size);
            return Ok(response);
        }

        [HttpPut(ApiEndPointConstant.WarrantyCard.WarrantyCardEndPoint)]
        [ProducesResponseType(typeof(WarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request)
        {
            var response = await _warrantyCardService.UpdateWarrantyCard(id, request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.WarrantyCard.WarrantyCardEndPoint)]
        [ProducesResponseType(typeof(WarrantyCardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarrantyCardById(int id)
        {
            var response = await _warrantyCardService.GetWarrantyCardById(id);
            return Ok(response);
        }
    }
}
