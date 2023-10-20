using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Dental
{
    public class DentalResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DentalStatus Status { get; set; }
        public int AccountId { get; set; }

        public DentalResponse(int id, string name, string address, DentalStatus status, int accountId)
        {
            Id = id;
            Name = name;
            Address = address;
            Status = status;
            AccountId = accountId;
        }
    }
}
