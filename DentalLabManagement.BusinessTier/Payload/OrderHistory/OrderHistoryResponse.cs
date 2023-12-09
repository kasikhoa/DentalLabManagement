using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderHistory
{
    public class OrderHistoryResponse
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }        
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }
        public OrderHistoryStatus Status { get; set; }

        public OrderHistoryResponse()
        {
        }

        public OrderHistoryResponse(DateTime createdDate, string createdBy, DateTime? completedDate, string? note, OrderHistoryStatus status)
        {
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            CompletedDate = completedDate;
            Note = note;
            Status = status;
        }
    }
}
