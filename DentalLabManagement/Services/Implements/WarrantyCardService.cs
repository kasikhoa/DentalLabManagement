using AutoMapper;
using DentalLabManagement.API.Extensions;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.WarrantyCard;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class WarrantyCardService : BaseService<WarrantyCardService>, IWarrantyCardService
    {
        public WarrantyCardService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<WarrantyCardService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<int> CreateNewWarrantyCard(CreateWarrantyCardRequest request)
        {
            CardType cardType = await _unitOfWork.GetRepository<CardType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.CardTypeId)) 
                ?? throw new BadHttpRequestException(MessageConstant.CardType.CardNotFoundMessage);
            
            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.CardTypeId.Equals(request.CardTypeId) && x.CardCode.Equals(request.CardCode)
                );
            if (warrantyCard != null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardCodeExistedMessage);

            warrantyCard = new WarrantyCard()
            {
                CardTypeId = request.CardTypeId,
                CardCode = request.CardCode,
                Status = WarrantyCardStatus.Valid.GetDescriptionFromEnum()
            };

            await _unitOfWork.GetRepository<WarrantyCard>().InsertAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CreateCardFailedMessage);

            return warrantyCard.Id;
        }

        private static Expression<Func<WarrantyCard, bool>> BuildWarrantyCardsQuery(WarrantyCardFilter filter)
        {
            Expression<Func<WarrantyCard, bool>> filterQuery = x => true;

            var cardCode = filter.cardCode;
            var cardTypeId = filter.cardTypeId;
            var status = filter.status;

            if (!string.IsNullOrEmpty(cardCode))
            {
                filterQuery = filterQuery.AndAlso(x => x.CardCode.Contains(cardCode));
            }

            if (cardTypeId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CardTypeId.Equals(cardTypeId));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<WarrantyCardResponse>> GetWarrantyCards(WarrantyCardFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<WarrantyCardResponse> result = await _unitOfWork.GetRepository<WarrantyCard>().GetPagingListAsync(
                selector: x => new WarrantyCardResponse()
                {
                    Id = x.Id,
                    OrderId = x.OrderItems.FirstOrDefault().OrderId,
                    CardCode = x.CardCode,
                    CategoryName = x.CardType.Category.Name,
                    CountryOrigin = x.CardType.CountryOrigin,
                    TeethQuantity = x.OrderItems.Count,
                    TeethPositions = x.OrderItems.Select(x => x.TeethPosition.PositionName).ToList(),                   
                    PatientName = x.OrderItems.FirstOrDefault().Order.PatientName,
                    DentalName = x.OrderItems.FirstOrDefault().Order.Partner.Name,
                    DentistName = x.OrderItems.FirstOrDefault().Order.DentistName,
                    StartDate = x.OrderItems.FirstOrDefault().Order.CompletedDate,
                    ExpDate = x.ExpDate,
                    Description = x.CardType.Description,
                    Image = x.CardType.Image,
                    BrandUrl = x.CardType.BrandUrl,
                    Status = EnumUtil.ParseEnum<WarrantyCardStatus>(x.Status)
                },
                predicate: BuildWarrantyCardsQuery(filter),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<WarrantyCardResponse> GetWarrantyCardById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.WarrantyCard.EmptyCardIdMessage);

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.CardType).ThenInclude(x => x.Category).Include(x => x.OrderItems).ThenInclude(x => x.TeethPosition)
                    .Include(x => x.OrderItems).ThenInclude(x => x.Order).ThenInclude(x => x.Partner)
                );
            if (warrantyCard == null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotFoundMessage);

            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                OrderId = warrantyCard.OrderItems.FirstOrDefault()?.OrderId,
                CardCode = warrantyCard.CardCode,
                CategoryName = warrantyCard.CardType.Category.Name,
                CountryOrigin = warrantyCard.CardType.CountryOrigin,
                TeethQuantity = warrantyCard.OrderItems.Count,
                TeethPositions = warrantyCard.OrderItems.Select(x => x.TeethPosition.PositionName).ToList(),               
                PatientName = warrantyCard.OrderItems.FirstOrDefault()?.Order.PatientName,
                DentalName = warrantyCard.OrderItems.FirstOrDefault()?.Order.Partner.Name,
                DentistName = warrantyCard.OrderItems.FirstOrDefault()?.Order.DentistName,
                StartDate = warrantyCard.OrderItems.FirstOrDefault()?.Order.CompletedDate,
                ExpDate = warrantyCard.ExpDate,
                Description = warrantyCard.CardType.Description,
                Image = warrantyCard.CardType.Image,
                BrandUrl = warrantyCard.CardType.BrandUrl,
                Status = EnumUtil.ParseEnum<WarrantyCardStatus>(warrantyCard.Status)
            };
        }

        public async Task<bool> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.WarrantyCard.EmptyCardIdMessage);           

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
               predicate: x => x.Id.Equals(id)
               );
            if (warrantyCard == null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotFoundMessage);

            CardType cardType = await _unitOfWork.GetRepository<CardType>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.CardTypeId)
                );
            if (cardType == null) throw new BadHttpRequestException(MessageConstant.CardType.CardNotFoundMessage);

            request.CardCode?.Trim();
            warrantyCard.CardTypeId = (request.CardTypeId < 1) ? warrantyCard.CardTypeId: request.CardTypeId;
            warrantyCard.CardCode = string.IsNullOrEmpty(request.CardCode) ? warrantyCard.CardCode : request.CardCode;
            warrantyCard.ExpDate = string.IsNullOrEmpty(request.ExpDate.ToString()) ? warrantyCard.ExpDate : request.ExpDate;

            _unitOfWork.GetRepository<WarrantyCard>().UpdateAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<WarrantyCardResponse> GetWarrantyCardByCode(string cardCode, string cardTypeCode)
        {
            cardCode = cardCode.Trim().ToLower();
            cardTypeCode = cardTypeCode.Trim().ToLower();

            Expression<Func<WarrantyCard, bool>> searchFilter = p => p.CardCode.Equals(cardCode) 
                && p.CardType.CodeName.Equals(cardTypeCode);

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: searchFilter,
                include: x => x.Include(x => x.CardType).ThenInclude(x => x.Category).Include(x => x.OrderItems)
                .ThenInclude(x => x.TeethPosition).Include(x => x.OrderItems)
                .ThenInclude(x => x.Order).ThenInclude(x => x.Partner)
                );
            if (warrantyCard == null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotFoundMessage);


            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                OrderId = warrantyCard.OrderItems.FirstOrDefault()?.OrderId,
                CardCode = warrantyCard.CardCode,
                CategoryName = warrantyCard.CardType.Category.Name,
                CountryOrigin = warrantyCard.CardType.CountryOrigin,
                TeethQuantity = warrantyCard.OrderItems.Count,
                TeethPositions = warrantyCard.OrderItems.Select(x => x.TeethPosition.PositionName).ToList(),
                PatientName = warrantyCard.OrderItems.FirstOrDefault()?.Order.PatientName,
                DentalName = warrantyCard.OrderItems.FirstOrDefault()?.Order.Partner.Name,
                DentistName = warrantyCard.OrderItems.FirstOrDefault()?.Order.DentistName,
                StartDate = warrantyCard.OrderItems.FirstOrDefault()?.Order.CompletedDate,
                ExpDate = warrantyCard.ExpDate,
                Description = warrantyCard.CardType.Description,
                Image = warrantyCard.CardType.Image,
                BrandUrl = warrantyCard.CardType.BrandUrl,
                Status = EnumUtil.ParseEnum<WarrantyCardStatus>(warrantyCard.Status)
            };
        }
    }
}
