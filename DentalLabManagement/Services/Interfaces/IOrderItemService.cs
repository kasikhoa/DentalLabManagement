using DentalLabManagement.BusinessTier.Payload.OrderItem;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderItemService
    {
        public Task<IPaginate<GetOrderItemResponse>> GetOrderItems(int? orderId, string? warrantyCardCode, int page, int size);
        public Task<GetOrderItemResponse> UpdateOrderItem(int id, UpdateOrderItemRequest request);
    }
}
