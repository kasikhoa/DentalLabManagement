using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class staff
    {
        public int Id { get; set; }
        public int? StageId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public int? AccountId { get; set; }

        public virtual ProductStage? Stage { get; set; }
    }
}
