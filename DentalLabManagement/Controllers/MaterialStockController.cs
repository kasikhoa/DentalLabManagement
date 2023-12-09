using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.MaterialStock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class MaterialStockController : BaseController<MaterialStockController>
    {
        private readonly IMaterialStockService _materialStockService;

        public MaterialStockController(ILogger<MaterialStockController> logger, IMaterialStockService materialStockService) : base(logger)
        {
            _materialStockService = materialStockService;
        }

        [HttpPost(ApiEndPointConstant.MaterialStock.MaterialStocksEndPoint)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewMaterial(MaterialStockRequest request)
        {
            var response = await _materialStockService.CreateNewMaterial(request);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.MaterialStock.MaterialStocksEndPoint)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewMaterialStock([FromQuery] MaterialStockFilter filter, int page, int size)
        {
            var response = await _materialStockService.ViewMaterialStock(filter, page, size);
            return Ok(response);
        }
    }
}
