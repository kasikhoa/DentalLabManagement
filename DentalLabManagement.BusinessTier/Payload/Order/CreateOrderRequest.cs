using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class CreateOrderRequest
    {
        public int DentalId { get; set; }     
        public string? DentistName { get; set; }
        public string PatientName { get; set; }
        public PatientGender? PatientGender { get; set; }
        public string? PatientPhoneNumber { get; set; }
        public string? DentistNote { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderProduct> ProductsList { get; set; } = new List<OrderProduct>();
        public double TotalAmount { get; set; }
        public double Discount { get; set; }
        public double FinalAmount { get; set; }
        public string? Note { get; set; }
       
    }

    public class OrderProduct
    {
        public int ProductId { get; set; }
        public int TeethPositionId { get; set; }
        public float TotalAmount { get; set; }
        public string? Note { get; set; }
    }
}
