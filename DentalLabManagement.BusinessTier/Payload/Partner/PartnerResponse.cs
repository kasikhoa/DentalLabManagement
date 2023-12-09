using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Partner
{
    public class PartnerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public PartnerStatus Status { get; set; }
        public PartnerType Type { get; set; }
        public int? AccountId { get; set; }

        public PartnerResponse(int id, string name, string address, PartnerStatus status, PartnerType type, int? accountId)
        {
            Id = id;
            Name = name;
            Address = address;
            Status = status;
            Type = type;
            AccountId = accountId;
        }
    }
}
