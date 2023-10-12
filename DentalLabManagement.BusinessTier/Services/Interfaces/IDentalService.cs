using DentalLabManagement.BusinessTier.Payload.Dental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Interfaces
{
    public interface IDentalService
    {
        public Task<DentalResponse> CreateDentalAccount(DentalRequest dentalRequest);
    }
}
