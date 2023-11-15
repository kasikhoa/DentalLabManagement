using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Payment
{
    public class PaymentFilter
    {
        public PaymentType? paymentType { get; set; }
        public PaymentStatus? status { get; set; }

    }
}
