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
        Task<ProductResponse> CreateProduct(ProductRequest productRequest);
        Task<IPaginate<ProductResponse>> GetProducts(ProductFilter filter, int page, int size);
        Task<ProductResponse> GetProductById(int productId);
        Task<bool> UpdateProduct(int id, UpdateProductRequest updateProductRequest);
        Task<bool> UpdateProductStatus(int id);    
    }
}
