using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ProductStageMapping
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StageId { get; set; }
        public int IndexStage { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual ProductionStage Stage { get; set; } = null!;
    }
}
