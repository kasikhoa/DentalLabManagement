using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderItemStage
{
    public class UpdateOrderItemStageRequest
    {
        public OrderItemStageStatus Status { get; set; }
        public string? Note { get; set; }
        public string? Image { get; set; }
    }
}
