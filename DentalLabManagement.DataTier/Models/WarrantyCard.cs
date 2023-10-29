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
        public int CardTypeId { get; set; }
        public string? CardCode { get; set; }
        public DateTime? ExpDate { get; set; }
        public string Status { get; set; } = null!;

        public virtual CardType CardType { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
