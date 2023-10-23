using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Payment
{
    public class PaymentRequest
    {
        public string? Note { get; set; }
        public PaymentType Type { get; set; }
        public double Amount { get; set; }

    }
}
