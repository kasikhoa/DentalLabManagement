using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Payment
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string? Note { get; set; }
        public PaymentType PaymentType { get; set; }
        public double Amount { get; set; }
        public double Remaining { get; set; }
        public DateTime PaymentTime { get; set; }
        public PaymentStatus Status { get; set; }

        public PaymentResponse() { }
    }
}
