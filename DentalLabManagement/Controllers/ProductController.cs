using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.DataTier.Paginate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Validators;

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

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Product.ProductEndPoint)]
        public async Task<IActionResult> UpdateProductStatus(int id)
        {
            var isSuccessful = await _productService.UpdateProductStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Product.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Product.UpdateStatusSuccessMessage);
        }
       
    }
}
