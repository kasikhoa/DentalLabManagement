using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Category
    {
        public Category()
        {
            GroupStages = new HashSet<GroupStage>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<GroupStage> GroupStages { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
