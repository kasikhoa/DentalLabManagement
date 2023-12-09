using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class MaterialStock
    {
        public MaterialStock()
        {
            ImportMaterials = new HashSet<ImportMaterial>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public double SupplierPrice { get; set; }
        public int WeightType { get; set; }
        public string Status { get; set; } = null!;

        public virtual Category? Category { get; set; }
        public virtual ICollection<ImportMaterial> ImportMaterials { get; set; }
    }
}
