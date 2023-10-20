using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.WarrantyCard;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Repository.Interfaces;

namespace DentalLabManagement.API.Services.Implements
{
    public class WarrantyCardService : BaseService<WarrantyCardService>, IWarrantyCardService
    {
        public WarrantyCardService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<WarrantyCardService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<WarrantyCardResponse> InseartNewWarrantyCard(WarrantyCardRequest warrantyCardRequest)
        {
            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.CardCode.Equals(warrantyCardRequest.CardCode));
            if (warrantyCard != null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardCodeExistedMessage);

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(warrantyCardRequest.CardType));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            warrantyCard = new WarrantyCard()
            {
                CardCode = warrantyCardRequest.CardCode,
                CardType = warrantyCardRequest.CardType,
                Description = warrantyCardRequest.Description,
                Image = warrantyCardRequest.Image,
                LinkCategory = warrantyCardRequest.LinkCategory
            };

            await _unitOfWork.GetRepository<WarrantyCard>().InsertAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CreateCardFailedMessage);
            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                CardCode = warrantyCard.CardCode,
                CategoryName = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    selector: x => x.CategoryName, predicate: x => x.Id.Equals(warrantyCard.CardType)),
                Description = warrantyCard.Description,
                Image = warrantyCard.Image,
                LinkCategory = warrantyCard.LinkCategory
            };
        }
    }
}
