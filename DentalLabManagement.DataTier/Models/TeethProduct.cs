using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class TeethProduct
    {
        public TeethProduct()
        {
            OrderItemStages = new HashSet<OrderItemStage>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int StaffId { get; set; }
        public int TeethPositionId { get; set; }
        public int WarrantyId { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual Account Staff { get; set; } = null!;
        public virtual TeethPosition TeethPosition { get; set; } = null!;
        public virtual WarrantyCard Warranty { get; set; } = null!;
        public virtual ICollection<OrderItemStage> OrderItemStages { get; set; }
    }
}
