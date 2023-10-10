using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.ProductStage
{
    public class ProductStageResponse
    {
        public int Id { get; set; }
        public int IndexStage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductStageResponse(int id, int indexStage, string name, string description)
        {
            Id = id;
            IndexStage = indexStage;
            Name = name;
            Description = description;
        }
    }
}
