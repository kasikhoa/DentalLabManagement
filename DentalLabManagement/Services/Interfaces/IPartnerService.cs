using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Partner;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IPartnerService
    {
        Task<int> CreateNewPartner(PartnerRequest request);
        Task<PartnerResponse> GetPartnerById(int dentalId);
        Task<IPaginate<PartnerResponse>> GetAllPartners(PartnerFilter filter, int page, int size);
        Task<bool> UpdatePartnerInfo(int id, UpdatePartnerRequest request);       
        Task<bool> DeletePartner(int id);
    }
}
