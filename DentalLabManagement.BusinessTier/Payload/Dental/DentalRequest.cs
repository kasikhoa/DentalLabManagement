using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Dental
{
    public class DentalRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        
    }
}
