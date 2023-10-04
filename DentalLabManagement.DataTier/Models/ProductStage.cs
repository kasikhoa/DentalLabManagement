using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ProductStage
    {
        public int Id { get; set; }
        public int? GroupStagetId { get; set; }
        public string? Name { get; set; }
        public int? LevelStage { get; set; }
        public string? Description { get; set; }
        public int OrderItemStageId { get; set; }

        public virtual GroupStage? GroupStaget { get; set; }
        public virtual OrderItemStage OrderItemStage { get; set; } = null!;
    }
}
