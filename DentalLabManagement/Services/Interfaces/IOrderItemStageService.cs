using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderItemStageService
    {
        public Task<UpdateOrderItemStageResponse> UpdateOrderItemStage(int orderItemStageId, UpdateOrderItemStageRequest updateOrderItemStageRequest);
        public Task<IPaginate<OrderItemStageResponse>> GetOrderItemStages(int? orderItemId, int? staffId, int? indexStage, OrderItemStageStatus? status, int page, int size);
    }
}
