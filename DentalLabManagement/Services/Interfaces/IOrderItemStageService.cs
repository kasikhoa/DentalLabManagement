using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderItemStageService
    {
        Task<bool> UpdateOrderItemStage(int orderItemStageId, UpdateOrderItemStageRequest request);
        Task<IPaginate<OrderItemStageResponse>> GetOrderItemStages(OrderItemStageFilter filter, int page, int size);
        Task<OrderItemStageResponse> GetOrderItemStageById(int id);
    }
}
