﻿using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Payload.Payment;
using DentalLabManagement.BusinessTier.Payload.OrderHistory;
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
        Task<int> CreateNewOrder(CreateOrderRequest createOrderRequest);
        Task<GetOrderDetailResponse> GetOrderTeethDetail(int id);
        Task<IPaginate<GetOrdersResponse>> GetOrders(OrderFilter filter, int page, int size);
        Task<bool> UpdateOrderStatus(int orderId, UpdateOrderRequest updateOrderRequest);
        Task<PaymentResponse> UpdateOrderPayment(int orderId, PaymentRequest paymentRequest);
        Task<IPaginate<PaymentResponse>> GetOrderPayments(int orderId, PaymentFilter filter, int page, int size);
        Task<OrderHistoryResponse> CreateWarrantyRequest(int orderId, CreateWarrantyRequest request);
        Task<IPaginate<OrderHistoryResponse>> ViewOrderHistory(int orderId, OrderHistoryFilter filter, int page, int size);
    }
}
