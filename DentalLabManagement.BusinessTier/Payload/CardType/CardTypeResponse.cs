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
        public string CategoryName { get; set; }
        public int WarrantyYear { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? BrandUrl { get; set; }
        public CardTypeStatus Status { get; set; }

        public CardTypeResponse() { }
        public CardTypeResponse(int id, string categoryName, int warrantyYear, string? description, string? image, string? brandUrl, CardTypeStatus status)
        {
            Id = id;
            CategoryName = categoryName;
            WarrantyYear = warrantyYear;
            Description = description;
            Image = image;
            BrandUrl = brandUrl;
            Status = status;
        }
    }
}
