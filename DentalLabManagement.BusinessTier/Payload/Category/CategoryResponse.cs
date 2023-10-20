using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Category
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public CategoryStatus Status { get; set; }
        public string? Image { get; set; }

        public CategoryResponse(int id, string categoryName, string description, CategoryStatus status, string? image)
        {
            Id = id;
            CategoryName = categoryName;
            Description = description;
            Status = status;
            Image = image;
        }
    }
}
