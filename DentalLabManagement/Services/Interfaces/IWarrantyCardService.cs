using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.WarrantyCard;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IWarrantyCardService
    {
        Task<WarrantyCardResponse> InseartNewWarrantyCard(WarrantyCardRequest warrantyCardRequest);
        Task<IPaginate<WarrantyCardResponse>> GetWarrantyCards(string? cardCode, int? categoryId, WarrantyCardStatus? status, int page, int size);
        Task<WarrantyCardResponse> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request);
        Task<WarrantyCardResponse> GetWarrantyCardById(int id);
    }
}
