using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Dentist
    {
        public int Id { get; set; }
        public string DentistName { get; set; } = null!;
        public int DentalId { get; set; }

        public virtual Dental Dental { get; set; } = null!;
    }
}
