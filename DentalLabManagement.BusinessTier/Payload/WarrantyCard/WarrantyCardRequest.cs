using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.WarrantyCard
{
    public class WarrantyCardRequest
    {
        public string? CardCode { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public WarrantyCardStatus Status { get; set; }
        public int OrderId { get; set; }
     
    }
}
