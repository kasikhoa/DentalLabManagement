using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class OrderItem
    {
        public OrderItem()
        {
            OrderItemStages = new HashSet<OrderItemStage>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int TeethPositionId { get; set; }
        public int? WarrantyId { get; set; }
        public double? SellingPrice { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public double TotalAmount { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual TeethPosition TeethPosition { get; set; } = null!;
        public virtual WarrantyCard? Warranty { get; set; }
        public virtual ICollection<OrderItemStage> OrderItemStages { get; set; }
    }
}
