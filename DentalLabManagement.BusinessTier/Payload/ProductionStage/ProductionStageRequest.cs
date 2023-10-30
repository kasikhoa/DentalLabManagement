using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.ProductStage
{
    public class ProductionStageRequest
    {
        [Required(ErrorMessage = "Index Stage is missing")]
        public int IndexStage { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Execution Time is missing")]
        public double? ExecutionTime { get; set; }

    }
}
