using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Category
{
    public class UpdateCategoryRequest
    { 
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public CategoryStatus Status { get; set; }
        public string? Image { get; set; }
        public void TrimString()
        {
            CategoryName = CategoryName.Trim();
            Description = Description.Trim();
            Image = Image?.Trim();
        }

    }
}
