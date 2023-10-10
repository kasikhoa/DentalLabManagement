using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class GroupStage
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? StageId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual ProductStage? Stage { get; set; }
    }
}
