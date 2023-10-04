using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Account
{
    public class CreateNewAccountRequest
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is missing")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is missing")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is missing")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is missing")]
        public string Role { get; set; }
        [Required(ErrorMessage = "Phone is missing")]
        public string PhoneNumber { get; set; }
    }
}
