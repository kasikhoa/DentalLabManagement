using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.WarrantyCard;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IWarrantyCardService
    {
        public Task<WarrantyCardResponse> InseartNewWarrantyCard(WarrantyCardRequest warrantyCardRequest);
        public Task<IPaginate<WarrantyCardResponse>> GetWarrantyCards(string? cardCode, int? categoryId, WarrantyCardStatus? status, int page, int size);
        public Task<WarrantyCardResponse> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request);
        public Task<WarrantyCardResponse> GetWarrantyCardById(int id);
    }
}
