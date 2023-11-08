using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DentalLabManagement.BusinessTier.Enums;
using System.Linq.Expressions;
using DentalLabManagement.API.Extensions;

namespace DentalLabManagement.API.Services.Implements
{
    public class CategoryService : BaseService<CategoryService>, ICategoryService
    {
        public CategoryService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<CategoryService> logger, AutoMapper.IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest categoryRequest)
        {
            Category newCategory = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync
                (predicate: x => x.Name.Equals(categoryRequest.CategoryName)
                );
            if (newCategory != null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNameExisted);

            newCategory = new Category()
            {
                Name = categoryRequest.CategoryName,
                Description = categoryRequest.Description,
                Status = categoryRequest.Status.GetDescriptionFromEnum(),
                Image = categoryRequest.Image,
            };
            await _unitOfWork.GetRepository<Category>().InsertAsync(newCategory);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Category.CreateNewCategoryFailedMessage);
            return new CategoryResponse(newCategory.Id, newCategory.Name, newCategory.Description,
                categoryRequest.Status, newCategory.Image);
        }

        private Expression<Func<Category, bool>> BuildGetCategoriesQuery(string? searchCategoryName, CategoryStatus? status)
        {
            Expression<Func<Category, bool>> filterQuery = x => true; 

            if (!string.IsNullOrEmpty(searchCategoryName))
            {
                filterQuery = filterQuery.AndAlso(x => x.Name.Contains(searchCategoryName));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<CategoryResponse>> GetCategories(string? searchCategoryName, CategoryStatus? status, int page, int size)
        {
            searchCategoryName = searchCategoryName?.Trim().ToLower();

            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<CategoryResponse> categories = await _unitOfWork.GetRepository<Category>().GetPagingListAsync(
                selector: x => new CategoryResponse(x.Id, x.Name, x.Description, EnumUtil.ParseEnum<CategoryStatus>(x.Status), x.Image),
                predicate: BuildGetCategoriesQuery(searchCategoryName, status),
                page: page,
                size: size
                );
            return categories;
        }

        public async Task<CategoryResponse> GetCategoryById(int categoryId)
        {
            if (categoryId < 1) throw new BadHttpRequestException(MessageConstant.Category.EmptyCategoryIdMessage);
            Category category = await _unitOfWork.GetRepository<Category>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(categoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage); ;
            return new CategoryResponse(category.Id, category.Name, category.Description, 
                EnumUtil.ParseEnum<CategoryStatus>(category.Status), category.Image);
        }     

        public async Task<bool> UpdateCategoryInformation(int categoryId, UpdateCategoryRequest request)
        {
            if (categoryId < 1) throw new BadHttpRequestException(MessageConstant.Category.EmptyCategoryIdMessage);

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(categoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            request.TrimString();
            category.Name = string.IsNullOrEmpty(request.CategoryName) ? category.Name : request.CategoryName;
            category.Description = string.IsNullOrEmpty(request.Description) ? category.Description : request.Description;
            category.Status = request.Status.GetDescriptionFromEnum();
            category.Image = string.IsNullOrEmpty(request.Image) ? category.Image : request.Image;

            ICollection<Product> products = await _unitOfWork.GetRepository<Product>().GetListAsync(
                predicate: x => x.CategoryId.Equals(category.Id));

            if (category.Status.Equals(CategoryStatus.Active.GetDescriptionFromEnum()))
            {
                products = products.Select(product =>
                {
                    product.Status = ProductStatus.Available.GetDescriptionFromEnum();
                    return product;
                }).ToList();
            }
            if (category.Status.Equals(CategoryStatus.Inactive.GetDescriptionFromEnum()))
            {
                products = products.Select(product =>
                {
                    product.Status = ProductStatus.Unavailable.GetDescriptionFromEnum();
                    return product;
                }).ToList();
            }

            _unitOfWork.GetRepository<Product>().UpdateRange(products);
            _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> CategoryMappingProductStage(int categoryId, List<int> request)
        {
            List<int> currentExtraCategoriesId = (List<int>) await _unitOfWork.GetRepository<GroupStage>().GetListAsync(
            selector: x => x.ProductStageId,
            predicate: x => x.CategoryId.Equals(categoryId)
            );

            (List<int> idsToRemove, List<int> idsToAdd, List<int> idsToKeep) splittedExtraCategoriesIds 
                = CustomListUtil.splitIdsToAddAndRemove(currentExtraCategoriesId, request);
            //Handle add and remove to database
            if (splittedExtraCategoriesIds.idsToAdd.Count > 0)
            {
                List<GroupStage> extraCategoriesToInsert = new List<GroupStage>();
                splittedExtraCategoriesIds.idsToAdd.ForEach(id => extraCategoriesToInsert.Add(new GroupStage
                {
                    CategoryId = categoryId,
                    ProductStageId = id
                }));
                await _unitOfWork.GetRepository<GroupStage>().InsertRangeAsync(extraCategoriesToInsert);
            }

            if (splittedExtraCategoriesIds.idsToRemove.Count > 0)
            {
                List<GroupStage> extraCategoriesToDelete = new List<GroupStage>();
                extraCategoriesToDelete = (List<GroupStage>) await _unitOfWork.GetRepository<GroupStage>()
                    .GetListAsync(predicate: x => x.CategoryId.Equals(categoryId) && splittedExtraCategoriesIds.idsToRemove.Contains(x.ProductStageId));

                _unitOfWork.GetRepository<GroupStage>().DeleteRangeAsync(extraCategoriesToDelete);
            }
            bool isSuccesful = await _unitOfWork.CommitAsync() > 0;
            return isSuccesful;
        }

        public async Task<IPaginate<ProductionStageResponse>> GetProductStageByCategory(int categoryId, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            List<int> categoryIds = (List<int>) await _unitOfWork.GetRepository<GroupStage>().GetListAsync(
             selector: x => x.ProductStageId,
             predicate: x => x.CategoryId.Equals(categoryId)
             );

            IPaginate<ProductionStageResponse> productStageResponse = await _unitOfWork.GetRepository<ProductionStage>().GetPagingListAsync(
                selector: x => new ProductionStageResponse(x.Id, x.IndexStage, x.Name, x.Description, x.ExecutionTime),
                predicate: x => categoryIds.Contains(x.Id),
                orderBy: x => x.OrderBy(x => x.IndexStage),
                page: page,
                size: size
                );
            return productStageResponse;
        }

        public async Task<bool> UpdateCategoryStatus(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Category.EmptyCategoryIdMessage);

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);
            category.Status = CategoryStatus.Inactive.GetDescriptionFromEnum();

            ICollection<Product> products = await _unitOfWork.GetRepository<Product>().GetListAsync(
                predicate: x => x.CategoryId.Equals(category.Id));

            products = products.Select(product =>
            {
                product.Status = ProductStatus.Unavailable.GetDescriptionFromEnum();
                return product;
            }).ToList();

            _unitOfWork.GetRepository<Product>().UpdateRange(products);
            _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
