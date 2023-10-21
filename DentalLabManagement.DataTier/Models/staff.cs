using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class staff
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public int ProductStageId { get; set; }

        public virtual ProductStage ProductStage { get; set; } = null!;
    }
}
