using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ProductionStage
    {
        public ProductionStage()
        {
            Accounts = new HashSet<Account>();
            OrderItemStages = new HashSet<OrderItemStage>();
            ProductStageMappings = new HashSet<ProductStageMapping>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ExecutionTime { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<OrderItemStage> OrderItemStages { get; set; }
        public virtual ICollection<ProductStageMapping> ProductStageMappings { get; set; }
    }
}
