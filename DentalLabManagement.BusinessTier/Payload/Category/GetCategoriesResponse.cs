using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.NewFolder
{
    public class GetCategoriesResponse
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public GetCategoriesResponse(int id, string categoryName, string description)
        {
            Id = id;
            CategoryName = categoryName;
            Description = description;
        }
    }
}
