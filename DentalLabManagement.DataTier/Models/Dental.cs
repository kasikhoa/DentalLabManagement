using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Dental
    {
        public Dental()
        {
            Dentists = new HashSet<Dentist>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string DentalName { get; set; } = null!;
        public string? Address { get; set; }
        public int AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Dentist> Dentists { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
