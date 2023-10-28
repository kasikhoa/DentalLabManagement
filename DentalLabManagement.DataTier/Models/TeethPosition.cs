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
        public int ToothArch { get; set; }
        public string PositionName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Image { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
