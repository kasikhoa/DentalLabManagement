using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class ProductStageController : BaseController<ProductStageController>
    {
        private readonly IProductStageService _productStageService;

        public ProductStageController(ILogger<ProductStageController> logger, IProductStageService productStageService) : base(logger)
        {
            _productStageService = productStageService;
        }

        [HttpPost(ApiEndPointConstant.ProductStage.ProductStageEndPoint)]
        [ProducesResponseType(typeof(ProductStageResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateProduct(ProductStageRequest productRequest)
        {
            var response = await _productStageService.CreateProductStage(productRequest);
            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok(response);
        }
    }
}
