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
            WarrantyCards = new HashSet<WarrantyCard>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Image { get; set; }

        public virtual ICollection<GroupStage> GroupStages { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<WarrantyCard> WarrantyCards { get; set; }
    }
}
