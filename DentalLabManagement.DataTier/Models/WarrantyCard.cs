using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class WarrantyCard
    {
        public WarrantyCard()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string? PatientName { get; set; }
        public string CardCode { get; set; } = null!;
        public string CardType { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
