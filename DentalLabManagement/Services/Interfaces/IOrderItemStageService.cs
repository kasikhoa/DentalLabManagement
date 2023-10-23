using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderItemStageService
    {
        Task<UpdateOrderItemStageResponse> UpdateOrderItemStage(int orderItemStageId, UpdateOrderItemStageRequest updateOrderItemStageRequest);
        Task<IPaginate<OrderItemStageResponse>> GetOrderItemStages(int? orderItemId, int? staffId, int? indexStage, OrderItemStageStatus? status, int page, int size);
        Task<OrderItemStageResponse> GetOrderItemStageById(int id);
    }
}
