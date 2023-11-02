using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class OrderItemStage
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int StageId { get; set; }
        public int? StaffId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpectedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public string Status { get; set; } = null!;
        public string Mode { get; set; } = null!;
        public string? Note { get; set; }
        public string? Image { get; set; }

        public virtual OrderItem OrderItem { get; set; } = null!;
        public virtual Account? Staff { get; set; }
        public virtual ProductionStage Stage { get; set; } = null!;
    }
}
