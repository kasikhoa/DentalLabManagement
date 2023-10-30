using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Category
{
    public class CategoryRequest
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public CategoryStatus Status { get; set; }
        public string? Image { get; set; }
    }
}
