using DentalLabManagement.API.Constants;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.NewFolder;
using DentalLabManagement.BusinessTier.Payload.Product;
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
            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Product.ProductsEndPoint)]
        [ProducesResponseType(typeof(IPaginate<GetProductsResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllCategories([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
            var products = await _productService.GetProducts(name, page, size);
            return Ok(products);
        }

        [HttpGet(ApiEndPointConstant.Product.ProductEndPoint)]
        [ProducesResponseType(typeof(GetCategoriesResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }

        [HttpPut(ApiEndPointConstant.Product.ProductEndPoint)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateCategoryInformation(int id,[FromBody] UpdateProductRequest updateProductRequest)
        {
            var response = await _productService.UpdateProduct(id, updateProductRequest);

            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok(response);
        }
    }
}
