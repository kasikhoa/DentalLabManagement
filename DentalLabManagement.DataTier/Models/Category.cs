using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Category
    {
        public Category()
        {
            CardTypes = new HashSet<CardType>();
            GroupStages = new HashSet<GroupStage>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Image { get; set; }
        public string? LinkBrand { get; set; }

        public virtual ICollection<CardType> CardTypes { get; set; }
        public virtual ICollection<GroupStage> GroupStages { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
