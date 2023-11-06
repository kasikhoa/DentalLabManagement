using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItem;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<IPaginate<GetOrderItemResponse>> GetOrderItems(int? orderId, int? productId, int? teethPositionId, string? warrantyCardCode, OrderItemMode? mode, int page, int size);
        Task<bool> UpdateOrderItem(int id, UpdateOrderItemRequest request);
        Task<bool> InsertWarrantyCard(int id, InsertWarrantyCardRequest updateRequest);
        Task<GetOrderItemResponse> GetOrderItemById(int id);
        Task<bool> UpdateStatusToWarranty(int orderItemId, WarrantyOrderItemRequest request);
    }
}
