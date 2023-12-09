using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.MaterialStock
{
    public class MaterialStockResponse
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public double SupplierPrice { get; set; }       
        public MaterialWeightType? WeightType { get; set; }
        public MaterialStockStatus? Status { get; set; }

        public MaterialStockResponse(int id, int? categoryId, string? code, string? name, string? description, int quantity, 
            double supplierPrice, MaterialWeightType? weightType, MaterialStockStatus? status)
        {
            Id = id;
            CategoryId = categoryId;
            Code = code;
            Name = name;
            Description = description;
            Quantity = quantity;
            SupplierPrice = supplierPrice;
            WeightType = weightType;
            Status = status;
        }
    }
}
