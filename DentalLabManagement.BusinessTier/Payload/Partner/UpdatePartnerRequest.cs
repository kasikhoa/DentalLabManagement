using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Partner
{
    public class UpdatePartnerRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public PartnerStatus? Status { get; set; }
        public int AccountId { get; set; }

        public void TrimString()
        {
            Name = Name?.Trim();
            Address = Address?.Trim();
        }
    }
}
