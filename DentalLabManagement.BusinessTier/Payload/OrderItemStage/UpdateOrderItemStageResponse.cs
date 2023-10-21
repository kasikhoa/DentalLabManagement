using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderItemStage
{
    public class UpdateOrderItemStageResponse
    {
        public int OrderItemId { get; set; }
        public string? StaffName { get; set; }
        public int IndexStage { get; set; }
        public string StageName { get; set; }
        public double ExecutionTime { get; set; }
        public OrderItemStageStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }
        public string? Image { get; set; }
        public string Message { get; set; }

        public UpdateOrderItemStageResponse(int orderItemId, string? staffName, int indexStage, string stageName, double executionTime, OrderItemStageStatus status, DateTime startDate, DateTime? endDate, string? note, string? image, string message)
        {
            OrderItemId = orderItemId;
            StaffName = staffName;
            IndexStage = indexStage;
            StageName = stageName;
            ExecutionTime = executionTime;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            Note = note;
            Image = image;
            Message = message;
        }
    }
}
