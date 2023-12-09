using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            PromotionOrderMappings = new HashSet<PromotionOrderMapping>();
            PromotionProductMappings = new HashSet<PromotionProductMapping>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public double? MaxDiscount { get; set; }
        public double? MinConditionAmount { get; set; }
        public double? DiscountAmount { get; set; }
        public double? DiscountPercent { get; set; }
        public string? Status { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public int? DayFilter { get; set; }

        public virtual ICollection<PromotionOrderMapping> PromotionOrderMappings { get; set; }
        public virtual ICollection<PromotionProductMapping> PromotionProductMappings { get; set; }
    }
}
