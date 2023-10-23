using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            Payments = new HashSet<Payment>();
            WarrantyCards = new HashSet<WarrantyCard>();
        }

        public int Id { get; set; }
        public string? InvoiceId { get; set; }
        public int DentalId { get; set; }
        public string? DentistName { get; set; }
        public string? DentistNote { get; set; }
        public string? PatientName { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? PatientGender { get; set; }
        public string Mode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int TeethQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Note { get; set; }

        public virtual Dental Dental { get; set; } = null!;
        public virtual Account? UpdatedByNavigation { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<WarrantyCard> WarrantyCards { get; set; }
    }
}
