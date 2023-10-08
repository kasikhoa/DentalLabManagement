using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Account
    {
        public Account()
        {
            Dentals = new HashSet<Dental>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int AccountId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Dental> Dentals { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
