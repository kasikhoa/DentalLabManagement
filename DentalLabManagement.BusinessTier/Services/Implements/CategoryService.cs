using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.NewFolder;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Implements
{
    public class CategoryService : BaseService<CategoryService>, ICategoryService
    {

        public CategoryService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<CategoryService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<Category?> CreateCategory(CategoryRequest categoryRequest)
        {
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync
                (predicate: x => x.CategoryName.Equals(categoryRequest.CategoryName));
            if (category != null)
            {
                throw new HttpRequestException("Category is already exist");
            }
            Category newCategory = new Category()
            {
                CategoryName = categoryRequest.CategoryName,
                Description = categoryRequest.Description,
            };
            await _unitOfWork.GetRepository<Category>().InsertAsync(newCategory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessful)
            {
                return newCategory;
            }
            return null;
        }

        public async Task<IPaginate<GetCategoriesResponse>> GetCategories(string? searchCategoryName, int page, int size)
        {
            searchCategoryName = searchCategoryName?.Trim().ToLower();
            IPaginate<GetCategoriesResponse> categories = await _unitOfWork.GetRepository<Category>().GetPagingListAsync(
                selector: x => new GetCategoriesResponse(x.Id, x.CategoryName, x.Description),
                predicate: string.IsNullOrEmpty(searchCategoryName) ? x => true : x => x.CategoryName.ToLower().Contains(searchCategoryName),
                page: page,
                size: size
                );
            return categories;
        }

        public async Task<GetCategoriesResponse> GetCategoryById(int categoryId)
        {
            if (categoryId < 1) throw new HttpRequestException("Id is not valid");
            Category category = await _unitOfWork.GetRepository<Category>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(categoryId));
            if (category == null) throw new HttpRequestException("Category not found!");
            GetCategoriesResponse response = new GetCategoriesResponse(category.Id, category.CategoryName, category.Description);
            return response;
        }     

        public async Task<Category> UpdateCategoryInformation(int categoryId, UpdateCategoryRequest updateCategoryRequest)
        {
            if (categoryId < 1) throw new HttpRequestException("Id is not valid");
            Category category = await _unitOfWork.GetRepository<Category>()
                 .SingleOrDefaultAsync(predicate: x => x.Id.Equals(categoryId));
            if (category == null) throw new HttpRequestException("Category not found!");
            updateCategoryRequest.TrimString();
            category.CategoryName = string.IsNullOrEmpty(updateCategoryRequest.CategoryName) ? category.CategoryName : updateCategoryRequest.CategoryName;
            category.Description = string.IsNullOrEmpty(updateCategoryRequest.Description) ? category.Description : updateCategoryRequest.Description;
           
            _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessful)
            {
                return category;
            }
            return null;
        }

        public async Task<IPaginate<GetProductsInCategory>> GetProductsInCategory(int categoryId, int page, int size)
        {
            if (categoryId < 1) throw new HttpRequestException("Id is not valid");
            Category category = await _unitOfWork.GetRepository<Category>()
                 .SingleOrDefaultAsync(predicate: x => x.Id.Equals(categoryId));
            if (category == null) throw new HttpRequestException("Category not found!");

            IPaginate<GetProductsInCategory> productsResponse = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
                selector: x => new GetProductsInCategory(x.Id, x.Name, x.Description, x.CostPrice, x.CategoryId),
                predicate: x => x.CategoryId == categoryId,
                page: page,
                size: size,
                orderBy: x => x.OrderBy(x => x.CostPrice)
               );
            return productsResponse;
        }
    }
}
