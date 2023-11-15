using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Payload.ProductionStage;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class ProductionStageController : BaseController<ProductionStageController>
    {
        private readonly IProductionStageService _productStageService;

        public ProductionStageController(ILogger<ProductionStageController> logger, IProductionStageService productStageService) : base(logger)
        {
            _productStageService = productStageService;
        }

        [HttpPost(ApiEndPointConstant.ProductStage.ProductStagesEndPoint)]
        [ProducesResponseType(typeof(ProductionStageResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateProductionStage(ProductionStageRequest productRequest)
        {
            var response = await _productStageService.CreateProductionStage(productRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.ProductStage.ProductStagesEndPoint)]
        [ProducesResponseType(typeof(ProductionStageResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetProductionStages([FromQuery] ProductionStageFilter filter, [FromQuery] int page, [FromQuery] int size)
        {
            var productStage = await _productStageService.GetProductionStages(filter, page, size);
            return Ok(productStage);
        }


        [HttpGet(ApiEndPointConstant.ProductStage.ProductStageEndPoint)]
        [ProducesResponseType(typeof(ProductionStageResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetProductionStageById(int id)
        {
            var productStage = await _productStageService.GetProductionStageById(id);
            return Ok(productStage);
        }

        [HttpPut(ApiEndPointConstant.ProductStage.ProductStageEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateProductionStage(int id, [FromBody] UpdateProductionStageRequest request)
        {
            var isSuccessful = await _productStageService.UpdateProductionStage(id, request);
            if (!isSuccessful) return Ok(MessageConstant.ProductionStage.UpdateProductStageFailedMessage);
            return Ok(MessageConstant.ProductionStage.UpdateProductStageSuccessMessage);
        }
        
    }
}
