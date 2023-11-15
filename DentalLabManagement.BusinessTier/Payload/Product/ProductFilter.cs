using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Product
{
    public class ProductFilter
    {
        public int? categoryId { get; set; }
        public string? name { get; set; }
        public double? minPrice { get; set; }
        public double? maxPrice { get; set; }
        public ProductStatus? status { get; set; }

    }
}
