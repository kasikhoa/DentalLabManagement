using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ExportMaterial
    {
        public int Id { get; set; }
        public int MaterialStockId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExportDate { get; set; }
        public string? Note { get; set; }

        public virtual MaterialStock MaterialStock { get; set; } = null!;
    }
}
