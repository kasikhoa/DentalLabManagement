using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.DataTier.Paginate;
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
        public Task<DentalAccountResponse> GetAccountDentalById(int dentalId);
        public Task<IPaginate<DentalResponse>> GetDentalAccounts(string? name, int page, int size);
        public Task<DentalResponse> UpdateDentalInfo(int id, UpdateDentalRequest updateDentalRequest);
    }
}
