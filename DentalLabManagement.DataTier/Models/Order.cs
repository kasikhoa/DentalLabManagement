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
        }

        public int Id { get; set; }
        public string PatientName { get; set; } = null!;
        public string? Gender { get; set; }
        public int? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Status { get; set; } = null!;
        public string Mode { get; set; } = null!;
        public int DentalId { get; set; }
        public string NameDentist { get; set; } = null!;
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public double FinalAmount { get; set; }
        public string InvoiceId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public string UpdateBy { get; set; } = null!;
        public int PatientPhoneNumber { get; set; }

        public virtual Dental Dental { get; set; } = null!;
        public virtual Patient PatientPhoneNumberNavigation { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
