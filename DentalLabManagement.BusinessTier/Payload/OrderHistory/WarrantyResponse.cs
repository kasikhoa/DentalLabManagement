using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderHistory
{
    public class WarrantyResponse
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }        
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }
        public OrderStatus Status { get; set; }

        public WarrantyResponse()
        {
        }
        public WarrantyResponse(int id, DateTime createdDate, string createdBy, DateTime? completedDate, string? note, OrderStatus status)
        {
            Id = id;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            CompletedDate = completedDate;
            Note = note;
            Status = status;
        }
    }
}
