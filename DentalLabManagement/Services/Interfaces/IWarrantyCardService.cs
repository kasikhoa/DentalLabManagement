using DentalLabManagement.BusinessTier.Payload.WarrantyCard;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IWarrantyCardService
    {
        public Task<WarrantyCardResponse> InseartNewWarrantyCard(WarrantyCardRequest warrantyCardRequest);
    }
}
