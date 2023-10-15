using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class OrderItemStage
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int? StaffId { get; set; }
        public int IndexStage { get; set; }
        public string StageName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ExecutionTime { get; set; }
        public string Status { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }
        public string? Image { get; set; }

        public virtual OrderItem OrderItem { get; set; } = null!;
        public virtual Account? Staff { get; set; }
    }
}
