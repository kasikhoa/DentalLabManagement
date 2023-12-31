﻿using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public string? Note { get; set; }
        public string PaymentType { get; set; } = null!;
        public double Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public string Status { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
