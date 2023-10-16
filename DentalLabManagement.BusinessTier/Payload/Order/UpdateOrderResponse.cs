using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class UpdateOrderResponse
    {
        public OrderStatus Status { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Message { get; set; }

        public UpdateOrderResponse()
        {
        }

        public UpdateOrderResponse(OrderStatus status, string updatedBy, DateTime? updatedAt, string message)
        {
            Status = status;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            Message = message;
        }
    }
}
