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
        public DateTime? CompletedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }  
        public string? Note { get; set; }
        public string Message { get; set; }

        public UpdateOrderResponse()
        {
        }

        public UpdateOrderResponse(OrderStatus status, DateTime? completedTime, string updatedBy, DateTime? updatedAt, string message)
        {
            Status = status;
            CompletedTime = completedTime;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            Message = message;
        }

        public UpdateOrderResponse(OrderStatus status, DateTime? completedTime, string updatedBy, DateTime? updatedAt, string? note, string message)
        {
            Status = status;
            CompletedTime = completedTime;
            UpdatedBy = updatedBy;
            UpdatedAt = updatedAt;
            Note = note;
            Message = message;
        }
    }
}
