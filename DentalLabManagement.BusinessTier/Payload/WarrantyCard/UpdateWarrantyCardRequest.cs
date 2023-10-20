using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.WarrantyCard
{
    public class UpdateWarrantyCardRequest
    {
        public string? CardCode { get; set; }
        //public DateTime? ExpDate { get; set;}
        public string? Image { get; set; }
        public string? LinkCategory { get; set; }

        public void TrimString()
        {
            CardCode = CardCode?.Trim();
            Image= Image?.Trim();
            LinkCategory = LinkCategory?.Trim();
        }
        
    }
}
