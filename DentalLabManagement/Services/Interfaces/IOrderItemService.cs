using DentalLabManagement.BusinessTier.Payload.OrderItem;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<IPaginate<GetOrderItemResponse>> GetOrderItems(int? orderId, string? warrantyCardCode, int page, int size);
        Task<GetOrderItemResponse> UpdateOrderItem(int id, UpdateOrderItemRequest request);
        Task<GetOrderItemResponse> InsertWarrantyCard(int id, InsertWarrantyCardRequest updateRequest);
        Task<GetOrderItemResponse> GetOrderItemById(int id);
    }
}
