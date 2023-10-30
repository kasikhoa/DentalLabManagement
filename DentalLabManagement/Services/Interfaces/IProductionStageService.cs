using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IProductionStageService
    {
        Task<ProductionStageResponse> CreateProductionStage(ProductionStageRequest request);
        Task<IPaginate<ProductionStageResponse>> GetProductionStages(string? name, int? index, int page, int size);
        Task<ProductionStageResponse> GetProductionStageById(int id);
        Task<bool> UpdateProductionStage(int id, UpdateProductionStageRequest request);
        
    }
}
