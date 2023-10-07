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

        public GetCategoriesResponse(int id, string categoryName)
        {
            Id = id;
            CategoryName = categoryName;
        }
    }
}
