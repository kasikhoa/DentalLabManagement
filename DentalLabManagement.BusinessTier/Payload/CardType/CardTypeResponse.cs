using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.CardType
{
    public class CardTypeResponse
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CodeName { get; set; }
        public string CountryOrigin { get; set; }
        public int WarrantyYear { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? BrandUrl { get; set; }
        public CardTypeStatus Status { get; set; }

        public CardTypeResponse() { }

        public CardTypeResponse(int id, int categoryId, string codeName, string countryOrigin, int warrantyYear, string? description, string? image, string? brandUrl, CardTypeStatus status)
        {
            Id = id;
            CategoryId = categoryId;
            CodeName = codeName;
            CountryOrigin = countryOrigin;
            WarrantyYear = warrantyYear;
            Description = description;
            Image = image;
            BrandUrl = brandUrl;
            Status = status;
        }
    }
}
