using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.WarrantyCard;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IWarrantyCardService
    {
        Task<CreateWarrantyCardResponse> CreateNewWarrantyCard(CreateWarrantyCardRequest request);
        Task<IPaginate<WarrantyCardResponse>> GetWarrantyCards(string? cardCode, int? cardTypeId, 
            WarrantyCardStatus? status, int page, int size);
        Task<bool> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request);
        Task<WarrantyCardResponse> GetWarrantyCardById(int id);
        Task<WarrantyCardResponse> GetWarrantyCardByCode(string code, string cardTypeCode);
    }
}
