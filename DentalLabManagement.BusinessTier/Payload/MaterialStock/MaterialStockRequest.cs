using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.MaterialStock
{
    public class MaterialStockRequest
    {
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double SupplierPrice { get; set; }
        [EnumDataType(typeof(MaterialWeightType))]
        public MaterialWeightType WeightType { get; set; }       

    }
}
