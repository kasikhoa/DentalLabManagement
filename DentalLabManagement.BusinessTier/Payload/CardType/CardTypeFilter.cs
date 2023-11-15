using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.CardType
{
    public class CardTypeFilter
    {
        public int? categoryId { get; set; }
        public string? codeName { get; set; }
        public string? countryOrigin { get; set; }
        public CardTypeStatus? status { get; set; }
    }
}
