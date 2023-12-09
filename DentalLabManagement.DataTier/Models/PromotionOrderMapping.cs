using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class PromotionOrderMapping
    {
        public int Id { get; set; }
        public int? PromotionId { get; set; }
        public int? OrderId { get; set; }
        public double? DiscountAmount { get; set; }
        public int? Quantity { get; set; }
        public int? OrderItemId { get; set; }
        public string? EffectType { get; set; }
        public string? VoucherCode { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Promotion? Promotion { get; set; }
    }
}
