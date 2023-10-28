using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class CardType
    {
        public CardType()
        {
            WarrantyCards = new HashSet<WarrantyCard>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int WarrantyYear { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? BrandUrl { get; set; }
        public string Status { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<WarrantyCard> WarrantyCards { get; set; }
    }
}
