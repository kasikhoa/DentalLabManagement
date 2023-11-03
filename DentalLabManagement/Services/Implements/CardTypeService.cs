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

        public async Task<CardTypeResponse> CreateCardType(CardTypeRequest request)
        {
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            CardType newCardType = new CardType()
            {
                CategoryId = category.Id,
                Code = request.Code,
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

            return new CardTypeResponse(newCardType.Id, category.Name, newCardType.Code, newCardType.CountryOrigin, newCardType.WarrantyYear,
                newCardType.Description, newCardType.Image, newCardType.BrandUrl, EnumUtil.ParseEnum<CardTypeStatus>(newCardType.Status));
        }

        public async Task<CardTypeResponse> GetCardTypeById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.CardType.EmptyCardIdMessage);
            CardType cardType = await _unitOfWork.GetRepository<CardType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Category)
                );
            if (cardType == null) throw new BadHttpRequestException(MessageConstant.CardType.CardNotFoundMessage);
            return new CardTypeResponse(cardType.Id, cardType.Category.Name, cardType.Code, cardType.CountryOrigin, cardType.WarrantyYear, 
                cardType.Description, cardType.Image, cardType.BrandUrl, EnumUtil.ParseEnum<CardTypeStatus>(cardType.Status));
        }

        private Expression<Func<CardType, bool>> BuildGetCardTypesQuery(int? categoryId, string? code, CardTypeStatus? status)
        {
            Expression<Func<CardType, bool>> filterQuery = x => true;

            if (categoryId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CategoryId.Equals(categoryId));
            }

            if (!string.IsNullOrEmpty(code))
            {
                filterQuery = filterQuery.AndAlso(x => x.Code.Equals(code));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<CardTypeResponse>> GetCardTypes(int? categoryId, string? code, CardTypeStatus? status, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<CardTypeResponse> result = await _unitOfWork.GetRepository<CardType>().GetPagingListAsync(
                selector: x => new CardTypeResponse(x.Id, x.Category.Name, x.Code, x.CountryOrigin, x.WarrantyYear, x.Description,
                x.Image, x.BrandUrl, EnumUtil.ParseEnum<CardTypeStatus>(x.Status)),
                predicate: BuildGetCardTypesQuery(categoryId, code, status),
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
            cardType.Code = string.IsNullOrEmpty(request.Code) ? cardType.Code : request.Code;
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
