using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Category
{
    public class GetProductsInCategory
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CostPrice { get; set; }
        public int CategoryId { get; set; }

        public GetProductsInCategory(int productId, string name, string description, double costPrice, int categoryId)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            CostPrice = costPrice;
            CategoryId = categoryId;
        }
    }
}
