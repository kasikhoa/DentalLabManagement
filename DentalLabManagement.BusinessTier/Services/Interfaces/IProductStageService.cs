using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Interfaces
{
    public interface IProductStageService
    {
        public Task<ProductStageResponse> CreateProductStage(ProductStageRequest productStageRequest);
        public Task<IPaginate<ProductStageResponse>> GetProductStages(string? name, int page, int size);
        public Task <ProductStageResponse> GetProductStageByIndexStage (int index);
        public Task<ProductStageResponse> GetProductStageById(int id);
        public Task<ProductStageResponse> UpdateProductStage(int id, UpdateProductStageRequest updateProductStageRequest);
        public Task<IPaginate<ProductStageResponse>> GetProductStageByCategory(int categoryId, int page, int size);
    }
}
