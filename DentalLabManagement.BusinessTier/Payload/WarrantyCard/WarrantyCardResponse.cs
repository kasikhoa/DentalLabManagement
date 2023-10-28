using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.WarrantyCard
{
    public class WarrantyCardResponse
    {
        public int Id { get; set; }       
        public string? CardCode { get; set; }
        public string? CategoryName { get; set; }
        public string? PatientName { get; set; }
        public string? DentalName { get; set; }
        public string? DentistName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? BrandUrl { get; set; }
        public WarrantyCardStatus Status { get; set; }

        public WarrantyCardResponse()
        {
        }
    }
}
