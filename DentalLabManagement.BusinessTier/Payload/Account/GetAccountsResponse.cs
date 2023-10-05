using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Account
{
    public class GetAccountsResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }

        public GetAccountsResponse(int id, string username, string name, string role, string phoneNumber)
        {
            Id = id;
            Username = username;
            Name = name;
            Role = role;
            PhoneNumber = phoneNumber;
        }
    }

}
