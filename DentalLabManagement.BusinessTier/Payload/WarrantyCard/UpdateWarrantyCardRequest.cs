using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.WarrantyCard
{
    public class UpdateWarrantyCardRequest
    {
        public int CardTypeId { get; set; }
        public string? CardCode { get; set; }
        public DateTime? ExpDate { get; set; }
      
    }
}
