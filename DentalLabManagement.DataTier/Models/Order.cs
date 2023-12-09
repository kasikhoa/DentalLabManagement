using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderHistories = new HashSet<OrderHistory>();
            OrderItems = new HashSet<OrderItem>();
            Payments = new HashSet<Payment>();
            PromotionOrderMappings = new HashSet<PromotionOrderMapping>();
        }

        public int Id { get; set; }
        public string? InvoiceId { get; set; }
        public int PartnerId { get; set; }
        public string? DentistName { get; set; }
        public string? DentistNote { get; set; }
        public string? PatientName { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string PatientGender { get; set; } = null!;
        public string Status { get; set; } = null!;
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }

        public virtual Partner Partner { get; set; } = null!;
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PromotionOrderMapping> PromotionOrderMappings { get; set; }
    }
}
