using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderItemStage
{
    public class OrderItemStageResponse
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int? StaffId { get; set; }
        public int StageId { get; set; }
        public int IndexStage { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpectedTime { get; set; }               
        public DateTime? CompletedTime { get; set; }
        public OrderItemStageStatus Status { get; set; }
        public OrderItemStageMode Mode { get; set; }
        public string? Note { get; set; }
        public string? Image { get; set; }

        public OrderItemStageResponse()
        {
        }
       
    }
}
