using DentalLabManagement.API.Constants;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {

        public ProductService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<ProductService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<ProductResponse> CreateProduct(ProductRequest productRequest)
        {
            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync
                (predicate: x => x.Name.Equals(productRequest.Name));
            if (product != null)
            {
                throw new HttpRequestException("Product is already exist");
            }
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(productRequest.CategoryId));
            if (category == null)
            {
                //throw new HttpRequestException("Category does not exist");
                throw new HttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);
            }
            Product newProduct = new Product()
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                CostPrice = productRequest.CostPrice,
                CategoryId = productRequest.CategoryId,
            };

            await _unitOfWork.GetRepository<Product>().InsertAsync(newProduct);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) return null;
            return new ProductResponse(newProduct.Id, newProduct.Name, newProduct.Description, newProduct.CostPrice, newProduct.CategoryId);
        }      

        public async Task<IPaginate<GetProductsResponse>> GetProducts(string? searchProductName, int page, int size)
        {
            searchProductName = searchProductName?.Trim().ToLower();
            IPaginate<GetProductsResponse> productsResponse = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
                selector: x => new GetProductsResponse(x.Id, x.Name, x.Description, x.CostPrice, x.CategoryId),
                predicate: string.IsNullOrEmpty(searchProductName) ? x => true : x => x.Name.ToLower().Contains(searchProductName),
                page: page,
                size: size,
                orderBy :x => x.OrderBy(x => x.CostPrice)
               );
            return productsResponse;
        }

        public async Task<ProductResponse> GetProductById(int productId)
        {
            if (productId < 1) throw new HttpRequestException("Id is not valid");
            Product product = await _unitOfWork.GetRepository<Product>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(productId));
            if (product == null) throw new HttpRequestException("Product not found!");
            ProductResponse response = new ProductResponse(product.Id, product.Name, product.Description, product.CostPrice, product.CategoryId);
            return response;
        }

        public async Task<ProductResponse> UpdateProduct(int productId, UpdateProductRequest updateProductRequest)
        {
            if (productId < 1) throw new HttpRequestException("Id is not valid");
            Product product = await _unitOfWork.GetRepository<Product>()
                 .SingleOrDefaultAsync(predicate: x => x.Id.Equals(productId));
            if (product == null) throw new HttpRequestException("Product not found!");
            updateProductRequest.TrimString();

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync
               (predicate: x => x.Id.Equals(updateProductRequest.CategoryId));
            if (category == null) throw new HttpRequestException("Category does not exist");

            Product updateProduct = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(productId));
            if (updateProduct == null) throw new HttpRequestException(MessageConstant.Product.ProductNotFoundMessage);

            updateProduct.Name = updateProductRequest.Name;
            updateProduct.Description = updateProductRequest.Description;
            updateProduct.CostPrice = updateProductRequest.CostPrice;
            updateProduct.CategoryId = updateProductRequest.CategoryId;

            _unitOfWork.GetRepository<Product>().UpdateAsync(updateProduct);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) return null;
            return new ProductResponse
                (updateProduct.Id, updateProduct.Name, updateProduct.Description, updateProduct.CostPrice, updateProduct.CategoryId);
        }
    }
}
