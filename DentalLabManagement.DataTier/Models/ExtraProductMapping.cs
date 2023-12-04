using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ExtraProductMapping
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ExtraProductId { get; set; }

        public virtual Product ExtraProduct { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
