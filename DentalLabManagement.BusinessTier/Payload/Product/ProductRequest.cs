using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Product
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "Name is missing")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is missing")]
        public double CostPrice { get; set; }
        [Required(ErrorMessage = "Category Id is missing")]
        public int CategoryId { get; set; }
    }
}
