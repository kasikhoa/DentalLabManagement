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
        public int IndexStage { get; set; }
        public string StageName { get; set; }
        public string Description { get; set; }
        public double Execution { get; set; }
        public OrderItemStageStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }
        public string? Image { get; set; }

        public OrderItemStageResponse()
        {
        }

        public OrderItemStageResponse(int id, int orderItemId, int? staffId, int indexStage, string stageName, string description, double execution, OrderItemStageStatus status, DateTime startDate, DateTime? endDate, string? note, string? image)
        {
            Id = id;
            OrderItemId = orderItemId;
            StaffId = staffId;
            IndexStage = indexStage;
            StageName = stageName;
            Description = description;
            Execution = execution;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            Note = note;
            Image = image;
        }
       
    }
}
