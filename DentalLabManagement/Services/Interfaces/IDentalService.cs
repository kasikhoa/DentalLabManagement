using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IDentalService
    {
        Task<DentalResponse> CreateDentalAccount(DentalRequest dentalRequest);
        Task<DentalAccountResponse> GetAccountDentalById(int dentalId);
        Task<IPaginate<DentalResponse>> GetDentals(string? name, DentalStatus? status, int page, int size);
        Task<DentalResponse> UpdateDentalInfo(int id, UpdateDentalRequest updateDentalRequest);
        Task<bool> UpdateDentalStatus(int id);
    }
}
