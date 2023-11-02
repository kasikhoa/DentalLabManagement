using System;
using System.Collections.Generic;

namespace DentalLabManagement.DataTier.Models
{
    public partial class ProductionStage
    {
        public ProductionStage()
        {
            Accounts = new HashSet<Account>();
            GroupStages = new HashSet<GroupStage>();
            OrderItemStages = new HashSet<OrderItemStage>();
        }

        public int Id { get; set; }
        public int IndexStage { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double ExecutionTime { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<GroupStage> GroupStages { get; set; }
        public virtual ICollection<OrderItemStage> OrderItemStages { get; set; }
    }
}
