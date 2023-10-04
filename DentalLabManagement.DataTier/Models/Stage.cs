using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Stage
    {
        public int Id { get; set; }
        public string? StageName { get; set; }
        public string? Status { get; set; }
    }
}
