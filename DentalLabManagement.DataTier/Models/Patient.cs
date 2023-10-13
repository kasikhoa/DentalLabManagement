using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Patient
    {
        public Patient()
        {
            WarrantyCards = new HashSet<WarrantyCard>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<WarrantyCard> WarrantyCards { get; set; }
    }
}
