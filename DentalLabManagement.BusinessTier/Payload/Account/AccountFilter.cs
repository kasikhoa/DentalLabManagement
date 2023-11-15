using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Account
{
    public class AccountFilter
    {
        public string? username { get; set; }
        public RoleEnum? role { get; set; }
        public int? stageId { get; set; }
        public AccountStatus? status { get; set; }
    }
}
