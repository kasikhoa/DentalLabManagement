using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ProductStage
    {
        public ProductStage()
        {
            GroupStages = new HashSet<GroupStage>();
        }

        public int Id { get; set; }
        public int IndexStage { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<GroupStage> GroupStages { get; set; }
    }
}
