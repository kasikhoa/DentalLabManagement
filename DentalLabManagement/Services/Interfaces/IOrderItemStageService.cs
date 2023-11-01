using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderItemStageService
    {
        Task<bool> UpdateOrderItemStage(int orderItemStageId, UpdateOrderItemStageRequest request);
        Task<IPaginate<OrderItemStageResponse>> GetOrderItemStages(int? orderItemId, int? staffId, int? indexStage, OrderItemStageStatus? status, 
            OrderItemStageMode? mode, int page, int size);
        Task<OrderItemStageResponse> GetOrderItemStageById(int id);
    }
}
