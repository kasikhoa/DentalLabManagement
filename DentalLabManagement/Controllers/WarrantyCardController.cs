using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
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
    }
}
