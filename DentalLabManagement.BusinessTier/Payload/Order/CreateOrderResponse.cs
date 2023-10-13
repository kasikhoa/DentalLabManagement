using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class CreateOrderResponse
    {
        public int Id { get; set; }
        public string InvoiceId { get; set; }
        public string DentalName { get; set; }
        public string? PatientName { get; set; }
        public string? DentistNote { get; set; }
        public string Status { get; set; }
        public OrderMode Mode { get; set; }
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreatedDate { get; set; }

        public CreateOrderResponse(int id, string invoiceId, string dentalName, string? patientName, string? dentistNote, string status, OrderMode mode, double totalAmount, double discount, double finalAmount, DateTime createdDate)
        {
            Id = id;
            InvoiceId = invoiceId;
            DentalName = dentalName;
            PatientName = patientName;
            DentistNote = dentistNote;
            Status = status;
            Mode = mode;
            TotalAmount = totalAmount;
            Discount = discount;
            FinalAmount = finalAmount;
            CreatedDate = createdDate;
        }
    }
}
