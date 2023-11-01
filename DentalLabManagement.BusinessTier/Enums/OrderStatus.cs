using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Enums
{
    public enum OrderStatus
    {
        Pending,
        Producing,
        Completed,        
        Warranty,
        Canceled
    }

    public enum OrderHistoryStatus
    {
        Pending,
        Producing,
        Completed,
        WarrantyRequest,
        Warranty,
        CompletedWarranty,
        Canceled
    }
}
