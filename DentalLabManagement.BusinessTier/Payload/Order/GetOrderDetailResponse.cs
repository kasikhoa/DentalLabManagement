using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Payment;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class GetOrderDetailResponse
    {
        public int Id { get; set; }
        public string? InvoiceId { get; set; }
        public string? DentalName { get; set; }
        public string? DentistName { get; set; }
        public string? DentistNote { get; set; }
        public string? PatientName { get; set; }
        public PatientGender? PatientGender { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public OrderStatus Status { get; set; }
        public int TeethQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Note { get; set; }
        public List<OrderTeethResponse> ToothList { get; set; } = new List<OrderTeethResponse>();
        public List<PaymentResponse> PaymentList { get; set; } = new List<PaymentResponse>();
    }

    public class OrderTeethResponse
    {
        public int OrderTeethId { get; set; }
        public string? ProductName { get; set; }
        public string? TeethPosition { get; set; }
        public string? Note { get; set; }
        public double TotalAmount { get; set; }

    }
}
