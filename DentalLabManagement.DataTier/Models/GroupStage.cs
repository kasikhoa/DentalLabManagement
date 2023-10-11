using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class GroupStage
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ProductStageId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ProductStage ProductStage { get; set; } = null!;
    }
}
