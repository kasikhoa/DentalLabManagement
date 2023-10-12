using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.BusinessTier.Services.Implements;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class DentalController : BaseController<DentalController>
    {
        private readonly IDentalService _dentalService;

        public DentalController(ILogger<DentalController> logger, IDentalService dentalService) : base(logger)
        {
            _dentalService = dentalService;
        }

        [HttpPost(ApiEndPointConstant.Dental.DentalsEndPoint)]
        [ProducesResponseType(typeof(DentalResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateDentalAccount(DentalRequest dentalRequest)
        {
            var response = await _dentalService.CreateDentalAccount(dentalRequest);
            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok(response);

        }
    }
}
