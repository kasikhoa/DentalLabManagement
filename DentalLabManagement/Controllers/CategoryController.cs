using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Services.Implements;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class CategoryController : BaseController<CategoryController>
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) : base(logger)
        {
            _categoryService = categoryService;
        }

        [HttpPost(ApiEndPointConstant.Category.CategoriesEndpoint)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateCategory(CategoryRequest categoryRequest)
        {
            var response = await _categoryService.CreateCategory(categoryRequest);
            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Category.CategoriesEndpoint)]
        [ProducesResponseType(typeof(IPaginate<GetCategoriesResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllCategories([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
            var categories = await _categoryService.GetCategories(name, page, size);
            return Ok(categories);
        }

        [HttpGet(ApiEndPointConstant.Category.CategoryEndpoint)]
        [ProducesResponseType(typeof(GetCategoriesResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [HttpPut(ApiEndPointConstant.Category.CategoryEndpoint)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateCategoryInformation(int id, UpdateCategoryRequest updateCategoryRequest)
        {
            var response = await _categoryService.UpdateCategoryInformation(id, updateCategoryRequest);

            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok(response);
        }

        [HttpPost(ApiEndPointConstant.Category.CategoryMappingProductStage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CategoryMappingProductStage(int id, List<int> request)
        {
            bool isSuccessful = await _categoryService.CategoryMappingProductStage(id, request);
            if (!isSuccessful) return Ok(MessageConstant.Category.UpdateExtraCategoryFailedMessage);
            return Ok(MessageConstant.Category.UpdateExtraCategorySuccessfulMessage);
        }

        [HttpGet(ApiEndPointConstant.Category.CategoryMappingProductStage)]
        [ProducesResponseType(typeof(IPaginate<ProductStageResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductStageByCategory(int id, int page, int size)
        {
            var response = await _categoryService.GetProductStageByCategory(id, page, size);
            return Ok(response);
        }

    }
}
