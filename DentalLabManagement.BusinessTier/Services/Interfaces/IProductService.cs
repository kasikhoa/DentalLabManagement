using DentalLabManagement.BusinessTier.Payload.NewFolder;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductResponse> CreateProduct(ProductRequest productRequest);
        public Task<IPaginate<GetProductsResponse>> GetProducts(string? searchProductName, int page, int size);
        public Task<ProductResponse> GetProductById(int productId);
        public Task<ProductResponse> UpdateProduct(int id, UpdateProductRequest updateProductRequest);
    }
}
