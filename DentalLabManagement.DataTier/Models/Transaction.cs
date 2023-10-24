using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }

        public virtual Payment Payment { get; set; } = null!;
    }
}
