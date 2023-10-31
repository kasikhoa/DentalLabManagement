using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class OrderHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }

        public virtual Account CreatedByNavigation { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
