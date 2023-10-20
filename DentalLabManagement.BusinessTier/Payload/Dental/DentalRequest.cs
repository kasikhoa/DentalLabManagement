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
        [Required(ErrorMessage ="Dental Name is missing")]
        public string DentalName { get; set; }
        [Required(ErrorMessage = "Address is missing")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Status is missing")]
        public DentalStatus Status { get; set; }
        [Required(ErrorMessage = "Account Id is missing")]
        public int AccountId { get; set; }
        
    }
}
