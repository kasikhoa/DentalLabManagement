using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ProductionStage
    {
        public ProductionStage()
        {
            Employees = new HashSet<Employee>();
            GroupStages = new HashSet<GroupStage>();
        }

        public int Id { get; set; }
        public int IndexStage { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ExecutionTime { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<GroupStage> GroupStages { get; set; }
    }
}
