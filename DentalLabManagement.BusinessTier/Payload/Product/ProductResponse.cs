using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
       
        public string Name { get; set; }     
        public string Description { get; set; }      
        public double CostPrice { get; set; }
        public int CategoryId { get; set; }

        public ProductResponse(int id, string name, string description, double costPrice, int categoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            CostPrice = costPrice;
            CategoryId = categoryId;
        }
    }
}
