using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IProductStageService
    {
        public Task<ProductStageResponse> CreateProductStage(ProductStageRequest productStageRequest);
        public Task<IPaginate<ProductStageResponse>> GetProductStages(string? name, int? index, int page, int size);
        public Task<ProductStageResponse> GetProductStageById(int id);
        public Task<ProductStageResponse> UpdateProductStage(int id, UpdateProductStageRequest updateProductStageRequest);
        
    }
}
