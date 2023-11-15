using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderItemStage
{
    public class OrderItemStageFilter
    {
        public int? orderId { get; set; }
        public int? orderItemId { get; set; }
        public int? accountId { get; set; }
        public int? staffId { get; set; }
        public int? stageId { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? completedTime { get; set; }
        public OrderItemStageStatus? status { get; set; }
        public OrderItemStageMode? mode { get; set; }
    }
}
