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
        [Required(ErrorMessage = "Category Name is missing")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Category Description is missing")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Category Status is missing")]
        public CategoryStatus Status { get; set; }
        public string? Image { get; set; }
        public string? LinkBrand { get; set; }
    }
}
