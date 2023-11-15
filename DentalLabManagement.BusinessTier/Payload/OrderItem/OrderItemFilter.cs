using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderItem
{
    public class OrderItemFilter
    {
        public int? orderId { get; set; }
        public int? productId { get; set; }
        public int? teethPositionId { get; set; }
        public int? warrantyCardId { get; set; }
        public OrderItemMode? mode { get; set; }
    }
}
