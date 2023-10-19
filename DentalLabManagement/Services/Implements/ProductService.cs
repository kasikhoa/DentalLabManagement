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

namespace DentalLabManagement.API.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {

        public ProductService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<ProductService> logger) : base(unitOfWork, logger)
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
                Name = productRequest.Name,
                Description = productRequest.Description,
                CostPrice = productRequest.CostPrice,
                CategoryId = productRequest.CategoryId,
            };

            await _unitOfWork.GetRepository<Product>().InsertAsync(newProduct);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Product.CreateNewProductFailedMessage);
            return new ProductResponse(newProduct.Id, newProduct.Name, newProduct.Description, newProduct.CostPrice, newProduct.CategoryId);
        }      

        public async Task<IPaginate<ProductResponse>> GetProducts(string? searchProductName, int page, int size)
        {
            searchProductName = searchProductName?.Trim().ToLower();
            IPaginate<ProductResponse> productsResponse = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
                selector: x => new ProductResponse(x.Id, x.Name, x.Description, x.CostPrice, x.CategoryId),
                predicate: string.IsNullOrEmpty(searchProductName) ? x => true : x => x.Name.ToLower().Contains(searchProductName),
                page: page,
                size: size,
                orderBy :x => x.OrderBy(x => x.CostPrice)
               );
            return productsResponse;
        }

        public async Task<ProductResponse> GetProductById(int productId)
        {
            if (productId < 1) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);
            Product product = await _unitOfWork.GetRepository<Product>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(productId));
            if (product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);
            return new ProductResponse(product.Id, product.Name, product.Description, product.CostPrice, product.CategoryId);
        }

        public async Task<ProductResponse> UpdateProduct(int productId, UpdateProductRequest updateProductRequest)
        {
            if (productId < 1) throw new BadHttpRequestException(MessageConstant.Product.EmptyProductIdMessage);         

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(updateProductRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            Product updateProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(productId));
            if (updateProduct == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);

            updateProductRequest.TrimString();

            updateProduct.Name = string.IsNullOrEmpty(updateProductRequest.Name) ? updateProduct.Name : updateProductRequest.Name;
            updateProduct.Description = string.IsNullOrEmpty(updateProductRequest.Description) ? updateProduct.Description : updateProductRequest.Description;
            updateProduct.CostPrice = (updateProductRequest.CostPrice < 0) ? updateProduct.CostPrice : updateProductRequest.CostPrice;
            updateProduct.CategoryId = updateProductRequest.CategoryId;

            _unitOfWork.GetRepository<Product>().UpdateAsync(updateProduct);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Product.UpdateProductFailedMessage);
            return new ProductResponse(updateProduct.Id, updateProduct.Name, 
                updateProduct.Description, updateProduct.CostPrice, updateProduct.CategoryId);
        }

        public async Task<IPaginate<GetProductsInCategory>> GetProductsInCategory(int categoryId, int page, int size)
        {
            if (categoryId < 1) throw new BadHttpRequestException(MessageConstant.Category.EmptyCategoryIdMessage);
            Category category = await _unitOfWork.GetRepository<Category>()
                 .SingleOrDefaultAsync(predicate: x => x.Id.Equals(categoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

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
