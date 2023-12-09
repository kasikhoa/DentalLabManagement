using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Partner
    {
        public Partner()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int? AccountId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
