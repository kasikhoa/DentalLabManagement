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
using static System.Net.Mime.MediaTypeNames;

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
                predicate: x => x.Id.Equals(warrantyCardRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            warrantyCard = new WarrantyCard()
            {
                CardCode = warrantyCardRequest.CardCode,
                CategoryId = warrantyCardRequest.CategoryId,
                Description = warrantyCardRequest.Description,
                Image = warrantyCardRequest.Image,
                LinkCategory = warrantyCardRequest.LinkCategory,
                Status = WarrantyCardStatus.Valid.GetDescriptionFromEnum()
            };

            await _unitOfWork.GetRepository<WarrantyCard>().InsertAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CreateCardFailedMessage);
            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                CardCode = warrantyCard.CardCode,
                CategoryName = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    selector: x => x.Name, predicate: x => x.Id.Equals(warrantyCard.CategoryId)),
                Image = warrantyCard.Image,
                LinkCategory = warrantyCard.LinkCategory,
                Status = EnumUtil.ParseEnum<WarrantyCardStatus>(warrantyCard.Status)
            };
        }

        private Expression<Func<WarrantyCard, bool>> BuildWarrantyCardsQuery(string? cardCode, int? categoryId, WarrantyCardStatus? status)
        {
            Expression<Func<WarrantyCard, bool>> filterQuery = x => true;
            if (!string.IsNullOrEmpty(cardCode))
            {
                filterQuery = filterQuery.AndAlso(x => x.CardCode.Contains(cardCode));
            }

            if (categoryId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CategoryId.Equals(categoryId));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<WarrantyCardResponse>> GetWarrantyCards(string? cardCode, int? categoryId, WarrantyCardStatus? status, int page, int size)
        {
            cardCode = cardCode?.Trim().ToLower();

            IPaginate<WarrantyCardResponse> result = await _unitOfWork.GetRepository<WarrantyCard>().GetPagingListAsync(
                selector: x => new WarrantyCardResponse()
                {
                    Id = x.Id,
                    CardCode= x.CardCode,
                    CategoryName = x.Category.Name,
                    PatientName = x.PatientName,    
                    DentalName = x.DentalName,
                    LaboName = x.LaboName,
                    StartDate= x.StartDate,
                    ExpDate= x.ExpDate,
                    Description= x.Description,
                    Image = x.Image,
                    LinkCategory = x.LinkCategory,
                    Status = EnumUtil.ParseEnum<WarrantyCardStatus>(x.Status)
                },
                predicate: BuildWarrantyCardsQuery(cardCode, categoryId, status),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<WarrantyCardResponse> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.WarrantyCard.EmptyWarrantyCardIdMessage);

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (warrantyCard == null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotFoundMessage);

            request.TrimString(); TimeUtils.GetCurrentSEATime();

            warrantyCard.CardCode = string.IsNullOrEmpty(request.CardCode) ? warrantyCard.CardCode : request.CardCode;
            warrantyCard.Image = string.IsNullOrEmpty(request.Image) ? warrantyCard.Image : request.Image;
            warrantyCard.LinkCategory = string.IsNullOrEmpty(request.LinkCategory) ? warrantyCard.LinkCategory : request.LinkCategory;

            _unitOfWork.GetRepository<WarrantyCard>().UpdateAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.WarrantyCard.UpdateCardFailedMessage);
            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                CardCode = warrantyCard.CardCode,
                CategoryName = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                    selector: x => x.Name, predicate: x => x.Id.Equals(warrantyCard.CategoryId)),
                Description = warrantyCard.Description,
                Image = warrantyCard.Image,
                LinkCategory = warrantyCard.LinkCategory,
                Status = EnumUtil.ParseEnum<WarrantyCardStatus>(warrantyCard.Status)
            };

        }
    }
}
