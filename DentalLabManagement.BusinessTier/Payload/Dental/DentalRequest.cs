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
        public string DentalName { get; set; }
        public string Address { get; set; }
        public int AccountId { get; set; }
        
    }
}
