using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ImportMaterial
    {
        public int Id { get; set; }
        public int MaterialStockId { get; set; }
        public string BillCode { get; set; } = null!;
        public byte[]? Image { get; set; }
        public int Quantity { get; set; }
        public double PurchasePrice { get; set; }
        public DateTime ImportDate { get; set; }
        public string? Note { get; set; }

        public virtual MaterialStock MaterialStock { get; set; } = null!;
    }
}
