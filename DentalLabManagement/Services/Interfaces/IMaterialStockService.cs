using DentalLabManagement.BusinessTier.Payload.MaterialStock;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IMaterialStockService
    {
        Task<int> CreateNewMaterial(MaterialStockRequest request);
        Task<IPaginate<MaterialStockResponse>> ViewMaterialStock(MaterialStockFilter filter, int page, int size);
    }
}
