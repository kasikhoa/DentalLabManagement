using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Payload.Payment;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest createOrderRequest);
        Task<GetOrderDetailResponse> GetOrderTeethDetail(int id);
        Task<IPaginate<GetOrderDetailResponse>> GetOrders(string? InvoiceId, OrderMode? mode, OrderStatus? status, int page, int size);
        Task<UpdateOrderResponse> UpdateOrderStatus(int orderId, UpdateOrderRequest updateOrderRequest);
        Task<PaymentResponse> UpdateOrderPayment(int orderId, PaymentRequest paymentRequest);
    }
}
