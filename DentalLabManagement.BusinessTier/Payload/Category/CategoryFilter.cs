using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Category
{
    public class CategoryFilter
    {
        public string? name { get; set; }
        public CategoryStatus? status { get; set; }
    }
}
