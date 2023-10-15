using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Services.Implements;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.DataTier.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateProduct(ProductRequest productRequest)
        {
            var response = await _productService.CreateProduct(productRequest);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Product.ProductsEndPoint)]
        [ProducesResponseType(typeof(IPaginate<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllProducts([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
            var products = await _productService.GetProducts(name, page, size);
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
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateProductInformation(int id,[FromBody] UpdateProductRequest updateProductRequest)
        {
            var response = await _productService.UpdateProduct(id, updateProductRequest);

            if (response == null) return BadRequest(NotFound());
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Product.ProductsInCategory)]
        [ProducesResponseType(typeof(IPaginate<GetProductsInCategory>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsInCategory(int categoryId, [FromQuery] int page, [FromQuery] int size)
        {
            var response = await _productService.GetProductsInCategory(categoryId, page, size);
            return Ok(response);
        }

       
    }
}
