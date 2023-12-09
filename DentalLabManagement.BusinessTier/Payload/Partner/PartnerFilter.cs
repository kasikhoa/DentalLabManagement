using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Partner
{
    public class PartnerFilter
    {
        public string? name { get; set; }
        public string? address { get; set; }
        public PartnerType? type { get; set; }
        public PartnerStatus? status { get; set; }
    }
}
