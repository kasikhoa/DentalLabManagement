using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.ProductStage
{
    public class ProductStageRequest
    {
        [Required(ErrorMessage = "Index Stage is required")]
        public int IndexStage { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Execution Time is required")]
        public double? ExecutionTime { get; set; }

    }
}
