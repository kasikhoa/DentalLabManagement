using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.ProductStage
{
    public class ProductionStageResponse
    {
        public int Id { get; set; }
        public int IndexStage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? ExecutionTime { get; set; }

        public ProductionStageResponse(int id, int indexStage, string name, string description, double? executionTime)
        {
            Id = id;
            IndexStage = indexStage;
            Name = name;
            Description = description;
            ExecutionTime = executionTime;
        }
    }
}
