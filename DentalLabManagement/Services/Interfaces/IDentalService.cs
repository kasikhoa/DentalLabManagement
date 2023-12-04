using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.BusinessTier.Payload.Order;
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
        Task<DentalResponse> CreateDental(DentalRequest request);
        Task<DentalResponse> GetDentalById(int dentalId);
        Task<IPaginate<DentalResponse>> GetDentals(DentalFilter filter, int page, int size);
        Task<bool> UpdateDentalInfo(int id, UpdateDentalRequest request);       
        Task<bool> DeleteDental(int id);
    }
}
