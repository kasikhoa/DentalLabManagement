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
        Task<IPaginate<GetOrdersResponse>> GetOrders(string? invoiceId, string? dentalName, OrderMode? mode, 
            OrderStatus? status, OrderPaymentStatus? paymentStatus, int page, int size);
        Task<bool> UpdateOrderStatus(int orderId, UpdateOrderRequest updateOrderRequest);
        Task<PaymentResponse> UpdateOrderPayment(int orderId, PaymentRequest paymentRequest);
        Task<IPaginate<PaymentResponse>> GetOrderPayments(int orderId, PaymentType? type, PaymentStatus? status, int page, int size);
    }
}
