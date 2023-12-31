﻿using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Account
    {
        public Account()
        {
            OrderHistories = new HashSet<OrderHistory>();
            OrderItemStages = new HashSet<OrderItemStage>();
            Partners = new HashSet<Partner>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? CurrentStage { get; set; }
        public string Status { get; set; } = null!;

        public virtual ProductionStage? CurrentStageNavigation { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<OrderItemStage> OrderItemStages { get; set; }
        public virtual ICollection<Partner> Partners { get; set; }
    }
}
