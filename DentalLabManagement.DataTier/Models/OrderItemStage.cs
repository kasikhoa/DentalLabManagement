﻿using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class OrderItemStage
    {
        public int Id { get; set; }
        public int OrderItemId { get; set; }
        public int StaffId { get; set; }
        public string Status { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string? Note { get; set; }
        public byte[]? Image { get; set; }

        public virtual OrderItem OrderItem { get; set; } = null!;
        public virtual Account Staff { get; set; } = null!;
    }
}
