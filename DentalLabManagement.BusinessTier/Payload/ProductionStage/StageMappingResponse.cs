using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.ProductionStage
{
    public class StageMappingResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IndexStage { get; set; }
        public string Description { get; set; }
        public double? ExecutionTime { get; set; }

        public StageMappingResponse(int id, string name, int indexStage, string description, double? executionTime)
        {
            Id = id;
            Name = name;
            IndexStage = indexStage;
            Description = description;
            ExecutionTime = executionTime;
        }
    }
}
