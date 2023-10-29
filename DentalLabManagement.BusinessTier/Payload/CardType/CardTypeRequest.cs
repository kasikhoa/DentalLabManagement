using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.CardType
{
    public class CardTypeRequest
    {
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string CountryOrigin { get; set; }
        public int WarrantyYear { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public string? BrandUrl { get; set; }

    }
}
