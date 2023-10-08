using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class OrderItemStage
    {
        public OrderItemStage()
        {
            ProductStages = new HashSet<ProductStage>();
        }

        public int Id { get; set; }
        public int TeethProductId { get; set; }
        public string StageId { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string? Note { get; set; }
        public byte[]? Image { get; set; }

        public virtual OrderItem TeethProduct { get; set; } = null!;
        public virtual ICollection<ProductStage> ProductStages { get; set; }
    }
}
