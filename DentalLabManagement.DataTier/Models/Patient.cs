using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Orders = new HashSet<Order>();
            WarrantyCards = new HashSet<WarrantyCard>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<WarrantyCard> WarrantyCards { get; set; }
    }
}
