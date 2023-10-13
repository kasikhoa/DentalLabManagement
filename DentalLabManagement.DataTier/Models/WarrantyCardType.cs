using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class WarrantyCardType
    {
        public WarrantyCardType()
        {
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }
        public string CardCode { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte[]? Image { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
