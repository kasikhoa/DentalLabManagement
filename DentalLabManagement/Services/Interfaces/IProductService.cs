using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.ProductionStage;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
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
        Task<int> CreateNewProduct(ProductRequest productRequest);
        Task<IPaginate<ProductResponse>> GetProducts(ProductFilter filter, int page, int size);
        Task<ProductResponse> GetProductById(int productId);
        Task<bool> UpdateProduct(int id, UpdateProductRequest updateProductRequest);
        Task<bool> ProductStageMapping(int productId, List<ProductStageMappingRequest> request);
        Task<bool> AddExtraProductsToProduct(int productId, List<int> request);
        Task<IPaginate<StageMappingResponse>> GetStageByProduct(int productId, string? name, int? indexStage, int page, int size);
        Task<IPaginate<ProductResponse>> GetExtraProductsByProductId(int productId, int page, int size);
        Task<bool> DeleteProduct(int id);    
    }
}
