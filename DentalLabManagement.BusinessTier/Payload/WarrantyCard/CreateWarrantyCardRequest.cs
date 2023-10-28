using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.WarrantyCard
{
    public class CreateWarrantyCardRequest
    {
        public int CardTypeId { get; set; }
        public string? CardCode { get; set; }
     
    }
}
