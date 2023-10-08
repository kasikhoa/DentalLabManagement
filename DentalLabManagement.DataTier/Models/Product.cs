using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Product
    {
        public Product()
        {
            GroupStages = new HashSet<GroupStage>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double CostPrice { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<GroupStage> GroupStages { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
