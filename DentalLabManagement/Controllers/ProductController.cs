using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.DataTier.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Validators;
using DentalLabManagement.API.Services.Implements;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Payload.ProductionStage;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
        {
            _productService = productService;
        }

        [HttpPost(ApiEndPointConstant.Product.ProductsEndPoint)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateNewProduct(ProductRequest productRequest)
        {
            var response = await _productService.CreateNewProduct(productRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Product.ProductsEndPoint)]
        [ProducesResponseType(typeof(IPaginate<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllProducts([FromQuery] ProductFilter filter, [FromQuery] int page, [FromQuery] int size)
        {
            var products = await _productService.GetProducts(filter, page, size);
            return Ok(products);
        }

        [HttpGet(ApiEndPointConstant.Product.ProductEndPoint)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }

        [HttpPut(ApiEndPointConstant.Product.ProductEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateProductInformation(int id,[FromBody] UpdateProductRequest updateProductRequest)
        {
            var isSuccessful = await _productService.UpdateProduct(id, updateProductRequest);
            if (isSuccessful) return Ok(MessageConstant.Product.UpdateProductFailedMessage);
            return Ok(MessageConstant.Product.UpdateProductSuccessMessage);
        }

        [HttpPost(ApiEndPointConstant.Product.ProductStageMapping)]
        public async Task<IActionResult> CategoryMappingProductStage(int id, [FromBody] List<ProductStageMappingRequest> request)
        {
            bool isSuccessful = await _productService.ProductStageMapping(id, request);
            if (!isSuccessful) return Ok(MessageConstant.Product.StageForProductFailedMessage);
            return Ok(MessageConstant.Product.StageForProductSuccessfulMessage);
        }

        [HttpPost(ApiEndPointConstant.Product.ExtraProductEndPoint)]
        public async Task<IActionResult> AddExtraProductsToProduct(int id, [FromBody] List<int> request)
        {
            bool isSuccessful = await _productService.AddExtraProductsToProduct(id, request);
            if (!isSuccessful) return Ok(MessageConstant.Product.UpdateExtraProductFailedMessage);
            return Ok(MessageConstant.Product.UpdateExtraProductSuccessfulMessage);
        }

        [HttpGet(ApiEndPointConstant.Product.ProductStageMapping)]
        [ProducesResponseType(typeof(IPaginate<StageMappingResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStageByProduct(int id, string? name, int? indexStage, int page, int size)
        {
            var response = await _productService.GetStageByProduct(id, name, indexStage, page, size);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Product.ExtraProductEndPoint)]
        [ProducesResponseType(typeof(IPaginate<StageMappingResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExtraProductsByProductId(int id, int page, int size)
        {
            var response = await _productService.GetExtraProductsByProductId(id, page, size);
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Product.ProductEndPoint)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isSuccessful = await _productService.DeleteProduct(id);
            if (!isSuccessful) return Ok(MessageConstant.Product.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Product.UpdateStatusSuccessMessage);
        }
       
    }
}
