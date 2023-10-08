using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class TeethPosition
    {
        public TeethPosition()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Position { get; set; } = null!;
        public string ProductId { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
