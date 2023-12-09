using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Partner
{
    public class PartnerAccountResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? UserName { get; set; }
        public AccountStatus Status { get; set; }

        public PartnerAccountResponse(int id, string name, string address, string? userName, AccountStatus status)
        {
            Id = id;
            Name = name;
            Address = address;
            UserName = userName;
            Status = status;
        }
    }
}
