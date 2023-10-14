using DentalLabManagement.BusinessTier.Payload.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest createOrderRequest);
        public Task<GetOrderDetailResponse> GetOrderTeethDetals(int id);
        
    }
}
