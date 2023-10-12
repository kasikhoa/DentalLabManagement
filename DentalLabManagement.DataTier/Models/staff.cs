using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class staff
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int AccountId { get; set; }
    }
}
