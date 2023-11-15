using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Dental
{
    public class DentalFilter
    {
        public string? name { get; set; }
        public string? address { get; set; }
        public DentalStatus? status { get; set; }
    }
}
