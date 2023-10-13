using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Services.Implements;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.DataTier.Paginate;
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

        [HttpPost(ApiEndPointConstant.ProductStage.ProductStagesEndPoint)]
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

        [HttpGet(ApiEndPointConstant.ProductStage.ProductStagesEndPoint)]
        [ProducesResponseType(typeof(ProductStageResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllProductStage([FromQuery] string? name, [FromQuery] int? indexStage, [FromQuery] int page, [FromQuery] int size)
        {
            var productStage = await _productStageService.GetProductStages(name, indexStage , page, size);
            return Ok(productStage);
        }


        [HttpGet(ApiEndPointConstant.ProductStage.ProductStageEndPoint)]
        [ProducesResponseType(typeof(ProductStageResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetProductStageById(int id)
        {
            var productStage = await _productStageService.GetProductStageById(id);
            return Ok(productStage);
        }

        [HttpPut(ApiEndPointConstant.ProductStage.ProductStageEndPoint)]
        [ProducesResponseType(typeof(ProductStageResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateProductStage(int id, [FromBody] UpdateProductStageRequest updateProductStageRequest)
        {
            var productStage = await _productStageService.UpdateProductStage(id, updateProductStageRequest);
            return Ok(productStage);
        }
        
    }
}
