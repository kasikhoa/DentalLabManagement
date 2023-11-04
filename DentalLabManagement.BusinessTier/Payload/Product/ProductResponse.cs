using DentalLabManagement.BusinessTier.Enums;
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
        public string? CategoryName { get; set; }        
        public string Name { get; set; }     
        public string Description { get; set; }      
        public double CostPrice { get; set; }      
        public ProductStatus Status { get; set; }
        public string? Image { get; set; }

        public ProductResponse()
        {
        }
        public ProductResponse(int id, string? categoryName, string name, string description, double costPrice, ProductStatus status, string? image)
        {
            Id = id;
            CategoryName = categoryName;
            Name = name;
            Description = description;
            CostPrice = costPrice;
            Status = status;
            Image = image;
        }
    }
}
