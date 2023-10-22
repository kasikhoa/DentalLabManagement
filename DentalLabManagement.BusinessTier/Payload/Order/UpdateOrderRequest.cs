using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class UpdateOrderRequest
    {
        public OrderStatus Status { get; set; }
        public int? UpdatedBy { get; set; }
        public string? Note { get; set; }
    }
}
