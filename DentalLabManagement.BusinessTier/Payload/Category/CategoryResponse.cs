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

        public CategoryResponse(int id, string categoryName, string description)
        {
            Id = id;
            CategoryName = categoryName;
            Description = description;
        }
    }
}
