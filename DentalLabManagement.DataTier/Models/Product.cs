using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class Product
    {
        public Product()
        {
            ExtraProductMappingExtraProducts = new HashSet<ExtraProductMapping>();
            ExtraProductMappingProducts = new HashSet<ExtraProductMapping>();
            OrderItems = new HashSet<OrderItem>();
            ProductStageMappings = new HashSet<ProductStageMapping>();
            PromotionProductMappings = new HashSet<PromotionProductMapping>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double CostPrice { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Image { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<ExtraProductMapping> ExtraProductMappingExtraProducts { get; set; }
        public virtual ICollection<ExtraProductMapping> ExtraProductMappingProducts { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductStageMapping> ProductStageMappings { get; set; }
        public virtual ICollection<PromotionProductMapping> PromotionProductMappings { get; set; }
    }
}
