using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.CardType;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface ICardTypeService
    {
        Task<CardTypeResponse> CreateCardType(CardTypeRequest request);
        Task<IPaginate<CardTypeResponse>> GetCardTypes(CardTypeFilter filter, int page, int size);
        Task<CardTypeResponse> GetCardTypeById(int id);
        Task<bool> UpdateCardType(int id, UpdateCardRequest request);
        Task<bool> DeleteCardType(int id);
    }
}
