using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Dental
{
    public class DentalAccountResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? UserName { get; set; }
        public AccountStatus Status { get; set; }

        public DentalAccountResponse(int id, string name, string address, string? userName, AccountStatus status)
        {
            Id = id;
            Name = name;
            Address = address;
            UserName = userName;
            Status = status;
        }
    }
}
