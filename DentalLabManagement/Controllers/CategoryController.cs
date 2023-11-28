using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Validators;

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
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Category.CategoriesEndpoint)]
        [ProducesResponseType(typeof(IPaginate<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllCategories([FromQuery] CategoryFilter filter, [FromQuery] int page, [FromQuery] int size)
        {
            var categories = await _categoryService.GetCategories(filter, page, size);
            return Ok(categories);
        }

        [HttpGet(ApiEndPointConstant.Category.CategoryEndpoint)]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [HttpPut(ApiEndPointConstant.Category.CategoryEndpoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateCategoryInformation(int id, UpdateCategoryRequest request)
        {
            var isSuccessful = await _categoryService.UpdateCategoryInformation(id, request);
            if (!isSuccessful) return Ok(MessageConstant.Category.UpdateCategoryFailedMessage);
            return Ok(MessageConstant.Category.UpdateCategorySuccessMessage);
        }             

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Category.CategoryEndpoint)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isSuccessful = await _categoryService.DeleteCategory(id);
            if (!isSuccessful) return Ok(MessageConstant.Category.UpdateStatusFailedMessage);
            return Ok(MessageConstant.Category.UpdateStatusSuccessMessage);
        }

    }
}
