using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.API.Services.Implements;
using DentalLabManagement.BusinessTier.Enums;

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
        public async Task<IActionResult> ViewAllDentals([FromQuery] string? name, DentalStatus? status, [FromQuery] int page, [FromQuery] int size)
        {
            var dentals = await _dentalService.GetDentals(name, status, page, size);
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

        [HttpDelete(ApiEndPointConstant.Dental.DentalEndPoint)]
        public async Task<IActionResult> UpdateProductStatus(int id)
        {
            var isSuccessful = await _dentalService.UpdateDentalStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Dental.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Dental.UpdateStatusSuccessMessage);
        }
    }
}
