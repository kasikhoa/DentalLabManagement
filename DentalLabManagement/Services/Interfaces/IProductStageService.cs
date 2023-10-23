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
        Task<ProductStageResponse> CreateProductStage(ProductStageRequest productStageRequest);
        Task<IPaginate<ProductStageResponse>> GetProductStages(string? name, int? index, int page, int size);
        Task<ProductStageResponse> GetProductStageById(int id);
        Task<ProductStageResponse> UpdateProductStage(int id, UpdateProductStageRequest updateProductStageRequest);
        
    }
}
