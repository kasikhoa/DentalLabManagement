using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Order
{
    public class OrderFilter
    {
        public string? invoiceId { get; set; }
        public int? dentalId { get; set; }
        public string? dentistName { get; set; }
        public string? patientName { get; set; }        
        public string? patientPhoneNumber { get; set; }
        public OrderStatus? status { get; set; }
        public DateTime? createdDateFrom { get; set; }
        public DateTime? createdDateTo { get; set; }
        //public string? DateOperators { get; set; }

        //public string? DateType { get; set; }
        //public string? DateParam { get; set; }
        //public DateTime? From { get; set; }
        //public DateTime? To { get; set; }

    }
    
}
