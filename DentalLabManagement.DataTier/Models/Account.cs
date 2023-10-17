using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Account
    {
        public Account()
        {
            Dentals = new HashSet<Dental>();
            OrderItemStages = new HashSet<OrderItemStage>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<Dental> Dentals { get; set; }
        public virtual ICollection<OrderItemStage> OrderItemStages { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
