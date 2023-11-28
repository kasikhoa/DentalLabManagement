using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateCategory(CategoryRequest categoryRequest);
        Task<IPaginate<CategoryResponse>> GetCategories(CategoryFilter filter, int page, int size);
        Task<CategoryResponse> GetCategoryById(int id);
        Task<bool> UpdateCategoryInformation(int categoryId, UpdateCategoryRequest request);              
        Task<bool> DeleteCategory(int id);
    }
}
