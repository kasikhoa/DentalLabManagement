using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class GetOrdersResponse
    {
        public int Id { get; set; }
        public string? InvoiceId { get; set; }
        public string DentalName { get; set; }
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
        public OrderPaymentStatus? PaymentStatus { get; set; }

        public GetOrdersResponse()
        {
        }
    }
}
