using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.MaterialStock
{
    public class MaterialStockFilter
    {
        public int? categoryId { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public MaterialWeightType? weightType { get; set; }
        public MaterialStockStatus? status { get; set; }
    }
}
