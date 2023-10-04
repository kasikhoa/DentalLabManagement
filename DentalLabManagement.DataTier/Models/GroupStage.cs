using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class GroupStage
    {
        public GroupStage()
        {
            ProductStages = new HashSet<ProductStage>();
        }

        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string? StageId { get; set; }
        public int? IndexStage { get; set; }

        public virtual Product? Product { get; set; }
        public virtual ICollection<ProductStage> ProductStages { get; set; }
    }
}
