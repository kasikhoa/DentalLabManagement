using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Partner;
using DentalLabManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Validators;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class PartnerController : BaseController<PartnerController>
    {
        private readonly IPartnerService _dentalService;

        public PartnerController(ILogger<PartnerController> logger, IPartnerService dentalService) : base(logger)
        {
            _dentalService = dentalService;
        }

        [HttpPost(ApiEndPointConstant.Partner.DentalsEndPoint)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateNewPartner(PartnerRequest request)
        {
            var response = await _dentalService.CreateNewPartner(request);
            return Ok(response);

        }

        [HttpGet(ApiEndPointConstant.Partner.DentalEndPoint)]
        [ProducesResponseType(typeof(PartnerResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetDentalById(int id)
        {
            var response = await _dentalService.GetPartnerById(id);          
            return Ok(response);

        }    

        [HttpGet(ApiEndPointConstant.Partner.DentalsEndPoint)]
        [ProducesResponseType(typeof(PartnerResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllDentals([FromQuery] PartnerFilter filter, [FromQuery] int page, [FromQuery] int size)
        {
            var dentals = await _dentalService.GetAllPartners(filter, page, size);
            return Ok(dentals);
        }

        [HttpPut(ApiEndPointConstant.Partner.DentalEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateDentalInfo(int id, UpdatePartnerRequest request)
        {
            var isSuccessful = await _dentalService.UpdatePartnerInfo(id, request);
            if (!isSuccessful) return Ok(MessageConstant.Dental.UpdateDentalFailedMessage);
            return Ok(MessageConstant.Dental.UpdateDentalSuccessMessage);

        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Partner.DentalEndPoint)]
        public async Task<IActionResult> DeleteDental(int id)
        {
            var isSuccessful = await _dentalService.DeletePartner(id);
            if (!isSuccessful) return Ok(MessageConstant.Dental.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Dental.UpdateStatusSuccessMessage);
        }
    }
}
