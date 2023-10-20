using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.WarrantyCard
{
    public class WarrantyCardRequest
    {
        public string CardCode { get; set; }
        public int CardType { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string LinkCategory { get; set; }
     
    }
}
