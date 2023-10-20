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
        public string? CardCode { get; set; }
        public int CategoryId { get; set; }
        public string? PatientName { get; set; }
        public string? DentalName { get; set; }
        public string? LaboName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string Description { get; set; } = null!;
        public string? Image { get; set; }
        public string? LinkCategory { get; set; }
        public string? Status { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
