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
    }
}
