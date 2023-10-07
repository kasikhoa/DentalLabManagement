using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Product
    {
        public Product()
        {
            GroupStages = new HashSet<GroupStage>();
            TeethProducts = new HashSet<TeethProduct>();
        }

        public int Id { get; set; }
        public double SellingPrice { get; set; }
        public double CostPrice { get; set; }
        public string Description { get; set; } = null!;
        public string TeethId { get; set; } = null!;
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<GroupStage> GroupStages { get; set; }
        public virtual ICollection<TeethProduct> TeethProducts { get; set; }
    }
}
