using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.OrderItem
{
    public class UpdateOrderItemRequest
    {
        public int ProductId { get; set; }
        public int TeethPositionId { get; set; }
        public string? Note { get; set; }
        public double TotalAmount { get; set; }

        public void TrimString()
        {
            Note = Note?.Trim();
        }
    }
}
