using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class WarrantyCard
    {
        public WarrantyCard()
        {
            TeethProducts = new HashSet<TeethProduct>();
        }

        public int Id { get; set; }
        public string OrderItemsId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; } = null!;
        public virtual ICollection<TeethProduct> TeethProducts { get; set; }
    }
}
