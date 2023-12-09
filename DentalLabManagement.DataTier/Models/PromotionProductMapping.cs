using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class PromotionProductMapping
    {
        public int Id { get; set; }
        public int? PromotionId { get; set; }
        public int? ProductId { get; set; }
        public string? Status { get; set; }
        public int? Quantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Promotion? Promotion { get; set; }
    }
}
