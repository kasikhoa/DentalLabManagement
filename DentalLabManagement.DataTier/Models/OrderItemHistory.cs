using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class OrderItemHistory
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; } = null!;

        public virtual OrderItem OrderItem { get; set; } = null!;
    }
}
