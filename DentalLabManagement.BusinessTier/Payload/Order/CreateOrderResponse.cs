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
        public string? DentistName { get; set; }
        public string? DentistNote { get; set; }
        public string? PatientName { get; set; } 
        public PatientGender? PatientGender { get; set; }
        public OrderStatus Status { get; set; }
        public OrderMode Mode { get; set; }
        public int TeethQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public double FinalAmount { get; set; }
        public DateTime CreatedDate { get; set; }

        public CreateOrderResponse(int id, string invoiceId, string dentalName, string? dentistName, string? dentistNote, string? patientName, PatientGender? patientGender, OrderStatus status, OrderMode mode, int teethQuantity, double totalAmount, double discount, double finalAmount, DateTime createdDate)
        {
            Id = id;
            InvoiceId = invoiceId;
            DentalName = dentalName;
            DentistName = dentistName;
            DentistNote = dentistNote;
            PatientName = patientName;
            PatientGender = patientGender;
            Status = status;
            Mode = mode;
            TeethQuantity = teethQuantity;
            TotalAmount = totalAmount;
            Discount = discount;
            FinalAmount = finalAmount;
            CreatedDate = createdDate;
        }
    }
}
