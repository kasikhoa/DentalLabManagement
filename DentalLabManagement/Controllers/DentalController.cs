using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.API.Services.Interfaces;
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
            return Ok(response);

        }

        [HttpGet(ApiEndPointConstant.Dental.DentalEndPoint)]
        [ProducesResponseType(typeof(DentalAccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetAccountDentalById(int id)
        {
            var response = await _dentalService.GetAccountDentalById(id);          
            return Ok(response);

        }

        [HttpGet(ApiEndPointConstant.Dental.DentalsEndPoint)]
        [ProducesResponseType(typeof(DentalResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllDentals([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
            var dentals = await _dentalService.GetDentalAccounts(name, page, size);
            return Ok(dentals);
        }

        [HttpPut(ApiEndPointConstant.Dental.DentalEndPoint)]
        [ProducesResponseType(typeof(DentalAccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateDentalInfo(int id, UpdateDentalRequest updateDentalRequest)
        {
            var response = await _dentalService.UpdateDentalInfo(id, updateDentalRequest);
            return Ok(response);

        }
    }
}
