using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.BusinessTier.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DentalLabManagement.API.Extensions;
using AutoMapper;

namespace DentalLabManagement.API.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {
        public ProductService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<ProductResponse> CreateProduct(ProductRequest productRequest)
        {
            Product newProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync
                (predicate: x => x.Name.Equals(productRequest.Name));
            if (newProduct != null) throw new BadHttpRequestException(MessageConstant.Product.ProductNameExisted);

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(productRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            newProduct = new Product()
            {
                CategoryId = productRequest.CategoryId,
                Name = productRequest.Name,
                Description = productRequest.Description,
                CostPrice = productRequest.CostPrice,               
                Status = ProductStatus.Available.GetDescriptionFromEnum(),
                Image = productRequest.Image
            };

            await _unitOfWork.GetRepository<Product>().InsertAsync(newProduct);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Product.CreateNewProductFailedMessage);
            return new ProductResponse(newProduct.Id, category.Name, newProduct.Name, newProduct.Description, newProduct.CostPrice, 
                EnumUtil.ParseEnum<ProductStatus>(newProduct.Status), newProduct.Image);
        }

        private Expression<Func<Product, bool>> BuildGetProductsQuery(ProductFilter filter)
        {
            Expression<Func<Product, bool>> filterQuery = x => true;

            var searchName = filter.name;
            var categoryId = filter.categoryId;
            var status = filter.status;
            var minPrice = filter.minPrice;
            var maxPrice = filter.maxPrice;

            if (!string.IsNullOrEmpty(searchName))
            {
                filterQuery = filterQuery.AndAlso(x => x.Name.Contains(searchName));
            }

            if (categoryId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CategoryId.Equals(categoryId));
            }            
            
            if (minPrice.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CostPrice >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CostPrice <= maxPrice);
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<ProductResponse>> GetProducts(ProductFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<ProductResponse> productsResponse = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
                selector: x => new ProductResponse(x.Id, x.Category.Name, x.Name, x.Description, x.CostPrice, 
                    EnumUtil.ParseEnum<ProductStatus>(x.Status), x.Image),
                predicate: BuildGetProductsQuery(filter),
                page: page,
                size: size,
                orderBy :x => x.OrderBy(x => x.CostPrice)
               );
            return productsResponse;
        }

        public async Task<ProductResponse> GetProductById(int productId)
        {
            if (productId < 1) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(productId),
                include: x => x.Include(x => x.Category)
                );
            if (product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);

            return new ProductResponse(product.Id, product.Category.Name, product.Name, product.Description, product.CostPrice, 
                EnumUtil.ParseEnum<ProductStatus>(product.Status), product.Image);
        }

        public async Task<bool> UpdateProduct(int productId, UpdateProductRequest updateProductRequest)
        {
            if (productId < 1) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);         

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(updateProductRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            Product updateProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(productId));
            if (updateProduct == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);

            updateProductRequest.TrimString();

            updateProduct.CategoryId = updateProductRequest.CategoryId;
            updateProduct.Name = string.IsNullOrEmpty(updateProductRequest.Name) ? updateProduct.Name : updateProductRequest.Name;
            updateProduct.Description = string.IsNullOrEmpty(updateProductRequest.Description) ? updateProduct.Description : updateProductRequest.Description;
            updateProduct.CostPrice = (updateProductRequest.CostPrice < 1) ? updateProduct.CostPrice : updateProductRequest.CostPrice;           
            updateProduct.Status = updateProductRequest.Status.GetDescriptionFromEnum();
            updateProduct.Image = string.IsNullOrEmpty(updateProductRequest.Image) ? updateProduct.Image : updateProductRequest.Image;

            _unitOfWork.GetRepository<Product>().UpdateAsync(updateProduct);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateProductStatus(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);

            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);

            product.Status = ProductStatus.Unavailable.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
