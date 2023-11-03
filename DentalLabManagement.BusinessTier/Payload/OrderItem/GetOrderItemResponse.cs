using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderItem
{
    public class GetOrderItemResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public string TeethPosition { get; set; }       
        public string? Note { get; set; } 
        public double TotalAmount { get; set; }
        public string? WarrantyCardCode { get; set; }
        public OrderItemMode Mode { get; set; }

        public GetOrderItemResponse()
        {
        }

    }
}
