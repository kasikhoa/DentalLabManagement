using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class OrderFilter
    {
        public string? invoiceId { get; set; }
        public string? phoneNumber { get; set; }
    }
}
