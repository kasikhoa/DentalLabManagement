using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ProductStage
    {
        public ProductStage()
        {
            GroupStages = new HashSet<GroupStage>();
            staff = new HashSet<staff>();
        }

        public int Id { get; set; }
        public int IndexStage { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ExecutionTime { get; set; }

        public virtual ICollection<GroupStage> GroupStages { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
