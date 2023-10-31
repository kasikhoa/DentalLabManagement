using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderHistory
{
    public class UpdateWarrantyRequest
    {
        public OrderStatus Status { get; set; }
        
    }
}
