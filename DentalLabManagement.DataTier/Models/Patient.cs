using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
