using AutoMapper;
using DentalLabManagement.API.Extensions;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.CardType;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class CardTypeService : BaseService<CardTypeService>, ICardTypeService
    {
        public CardTypeService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<CardTypeService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<int> CreateCardType(CardTypeRequest request)
        {
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            CardType newCardType = await _unitOfWork.GetRepository<CardType>().SingleOrDefaultAsync(
                predicate: x => x.CodeName.Equals(request.CodeName));
            if (newCardType != null) throw new BadHttpRequestException(MessageConstant.CardType.CardExistedMessage);
          
            newCardType = new CardType()
            {
                CategoryId = category.Id,
                CodeName = request.CodeName,
                CountryOrigin = request.CountryOrigin,
                WarrantyYear = request.WarrantyYear,
                Description = request.Description,
                Image = request.Image,
                BrandUrl = request.BrandUrl,
                Status = CardTypeStatus.Active.GetDescriptionFromEnum()
            };

            await _unitOfWork.GetRepository<CardType>().InsertAsync(newCardType);
            bool isSucessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSucessful) throw new BadHttpRequestException(MessageConstant.CardType.CreateCardFailedMessage);

            return newCardType.Id;
        }

        public async Task<CardTypeResponse> GetCardTypeById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.CardType.EmptyCardIdMessage);
            CardType cardType = await _unitOfWork.GetRepository<CardType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Category)
                );
            if (cardType == null) throw new BadHttpRequestException(MessageConstant.CardType.CardNotFoundMessage);
            return new CardTypeResponse(cardType.Id, cardType.CategoryId, cardType.CodeName, cardType.CountryOrigin, cardType.WarrantyYear, 
                cardType.Description, cardType.Image, cardType.BrandUrl, EnumUtil.ParseEnum<CardTypeStatus>(cardType.Status));
        }

        private static Expression<Func<CardType, bool>> BuildGetCardTypesQuery(CardTypeFilter filter)
        {
            Expression<Func<CardType, bool>> filterQuery = x => true;

            var categoryId = filter.categoryId;
            var codeName = filter.codeName;
            var countryOrigin = filter.countryOrigin;
            var status = filter.status;

            if (categoryId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CategoryId.Equals(categoryId));
            }

            if (!string.IsNullOrEmpty(codeName))
            {
                filterQuery = filterQuery.AndAlso(x => x.CodeName.Contains(codeName));
            }

            if (!string.IsNullOrEmpty(countryOrigin))
            {
                filterQuery = filterQuery.AndAlso(x => x.CountryOrigin.Contains(countryOrigin));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<CardTypeResponse>> GetCardTypes(CardTypeFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<CardTypeResponse> result = await _unitOfWork.GetRepository<CardType>().GetPagingListAsync(
                selector: x => new CardTypeResponse(x.Id, x.CategoryId, x.CodeName, x.CountryOrigin, x.WarrantyYear, x.Description,
                x.Image, x.BrandUrl, EnumUtil.ParseEnum<CardTypeStatus>(x.Status)),
                predicate: BuildGetCardTypesQuery(filter),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<bool> UpdateCardType(int id, UpdateCardRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.CardType.EmptyCardIdMessage);
            CardType cardType = await _unitOfWork.GetRepository<CardType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
                );
            if (cardType == null) throw new BadHttpRequestException(MessageConstant.CardType.CardNotFoundMessage);

            request.TrimString();
            cardType.CodeName = string.IsNullOrEmpty(request.CodeName) ? cardType.CodeName : request.CodeName;
            cardType.CountryOrigin = string.IsNullOrEmpty(request.CountryOrigin) ? cardType.CountryOrigin : request.CountryOrigin;
            cardType.WarrantyYear = (request.WarrantyYear < 1) ? cardType.WarrantyYear : request.WarrantyYear;
            cardType.Description = string.IsNullOrEmpty(request.Description) ? cardType.Description : request.Description;
            cardType.Image = string.IsNullOrEmpty(request.Image) ? cardType.Image : request.Image;
            cardType.BrandUrl = string.IsNullOrEmpty(request.BrandUrl) ? cardType.BrandUrl : request.BrandUrl;
            cardType.Status = request.CardStatus.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<CardType>().UpdateAsync(cardType);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> DeleteCardType(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.CardType.EmptyCardIdMessage);
            CardType cardType = await _unitOfWork.GetRepository<CardType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
                );
            if (cardType == null) throw new BadHttpRequestException(MessageConstant.CardType.CardNotFoundMessage);

            cardType.Status = CardTypeStatus.Inactive.GetDescriptionFromEnum();
            _unitOfWork.GetRepository<CardType>().UpdateAsync(cardType);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
