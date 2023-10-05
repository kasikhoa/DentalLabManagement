using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Account
{
    public class CreateNewAccountResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
