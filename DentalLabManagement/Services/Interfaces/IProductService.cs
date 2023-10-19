using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductResponse> CreateProduct(ProductRequest productRequest);
        public Task<IPaginate<ProductResponse>> GetProducts(string? searchProductName, int? categoryId, ProductStatus? status, int page, int size);
        public Task<ProductResponse> GetProductById(int productId);
        public Task<ProductResponse> UpdateProduct(int id, UpdateProductRequest updateProductRequest);
        public Task<IPaginate<GetProductsInCategory>> GetProductsInCategory(int categoryId, int page, int size);
        public Task<bool> UpdateProductStatus(int id);
    }
}
