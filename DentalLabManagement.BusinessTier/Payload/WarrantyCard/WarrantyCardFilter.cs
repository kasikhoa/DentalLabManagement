using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.WarrantyCard
{
    public class WarrantyCardFilter
    {
        public string? cardCode { get; set; }
        public int? cardTypeId { get; set; }
        public WarrantyCardStatus? status { get; set; }
    }
}
