using DentalLabManagement.BusinessTier.Payload.ProductStage;
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
    }
}
