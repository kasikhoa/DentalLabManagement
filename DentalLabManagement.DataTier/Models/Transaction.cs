using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public string PaymentId { get; set; } = null!;
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
