using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.ProductStage
{
    public class UpdateProductStageRequest
    {
        public int IndexStage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double ExecutionTime { get; set; }

        public void TrimString()
        {
            Name = Name.Trim();
            Description = Description.Trim();
        }
    }
}
