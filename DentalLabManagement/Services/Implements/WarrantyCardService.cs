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
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class WarrantyCardService : BaseService<WarrantyCardService>, IWarrantyCardService
    {
        public WarrantyCardService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<WarrantyCardService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<WarrantyCardResponse> InseartNewWarrantyCard(WarrantyCardRequest warrantyCardRequest)
        {

            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(warrantyCardRequest.CategoryId));
            if (category == null) throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(warrantyCardRequest.OrderId),
                include: x => x.Include(x => x.Dental)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.CardCode.Equals(warrantyCardRequest.CardCode) && x.CategoryId.Equals(warrantyCardRequest.CategoryId));
            if (warrantyCard != null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardCodeExistedMessage);

            string cardCode = string.IsNullOrEmpty(warrantyCardRequest.CardCode) ? null : warrantyCardRequest.CardCode;

            warrantyCard = new WarrantyCard()
            {
                CardCode = cardCode,
                CategoryId = warrantyCardRequest.CategoryId,
                PatientName = order.PatientName,
                DentalName = order.Dental.Name,
                DentistName = order.DentistName,
                LaboName = order.LaboName,
                StartDate = TimeUtils.GetCurrentSEATime(),
                Description = warrantyCardRequest.Description,
                Image = category.Image,
                LinkCategory = category.LinkBrand,
                Status = WarrantyCardStatus.Valid.GetDescriptionFromEnum(),
                OrderId = order.Id
            };

            if (warrantyCard.CategoryId >= 1 && warrantyCard.CategoryId <= 4)
            {
                warrantyCard.CardCode = order.InvoiceId;
            }

            await _unitOfWork.GetRepository<WarrantyCard>().InsertAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CreateCardFailedMessage);
            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                CardCode = warrantyCard.CardCode,
                CategoryName = category.Name,
                PatientName = warrantyCard.PatientName,
                DentalName = warrantyCard.DentalName,
                DentistName = warrantyCard.DentistName,
                LaboName = warrantyCard.LaboName,
                StartDate = warrantyCard.StartDate,
                Description = warrantyCard.Description,
                Image = warrantyCard.Image,
                LinkCategory = warrantyCard.LinkCategory,
                Status = EnumUtil.ParseEnum<WarrantyCardStatus>(warrantyCard.Status),
                OrderId = warrantyCard.OrderId
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
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<WarrantyCardResponse> result = await _unitOfWork.GetRepository<WarrantyCard>().GetPagingListAsync(
                selector: x => new WarrantyCardResponse()
                {
                    Id = x.Id,
                    CardCode = x.CardCode,
                    CategoryName = x.Category.Name,
                    PatientName = x.PatientName,
                    DentalName = x.DentalName,
                    LaboName = x.LaboName,
                    StartDate = x.StartDate,
                    ExpDate = x.ExpDate,
                    Description = x.Description,
                    Image = x.Image,
                    LinkCategory = x.LinkCategory,
                    Status = EnumUtil.ParseEnum<WarrantyCardStatus>(x.Status),
                    OrderId = x.OrderId,
                },
                predicate: BuildWarrantyCardsQuery(cardCode, categoryId, status),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<WarrantyCardResponse> UpdateWarrantyCard(int id, UpdateWarrantyCardRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.WarrantyCard.EmptyCardIdMessage);

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Category)
                );
            if (warrantyCard == null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotFoundMessage);

            request.TrimString();

            warrantyCard.CardCode = string.IsNullOrEmpty(request.CardCode) ? warrantyCard.CardCode : request.CardCode;
            warrantyCard.LinkCategory = string.IsNullOrEmpty(request.LinkCategory) ? warrantyCard.LinkCategory : request.LinkCategory;

            _unitOfWork.GetRepository<WarrantyCard>().UpdateAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.WarrantyCard.UpdateCardFailedMessage);
            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                CardCode = warrantyCard.CardCode,
                CategoryName = warrantyCard.Category.Name,
                PatientName = warrantyCard.PatientName,
                DentalName = warrantyCard.DentalName,
                DentistName = warrantyCard.DentistName,
                StartDate = warrantyCard.StartDate,
                Description = warrantyCard.Description,
                Image = warrantyCard.Image,
                LinkCategory = warrantyCard.LinkCategory,
                Status = EnumUtil.ParseEnum<WarrantyCardStatus>(warrantyCard.Status),
                OrderId = warrantyCard.OrderId,
            };

        }

        public async Task<WarrantyCardResponse> GetWarrantyCardById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.WarrantyCard.EmptyCardIdMessage);

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Category));
            if (warrantyCard == null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotFoundMessage);

            return new WarrantyCardResponse()
            {
                Id = warrantyCard.Id,
                CardCode = warrantyCard.CardCode,
                CategoryName = warrantyCard.Category.Name,
                PatientName = warrantyCard.PatientName,
                DentalName = warrantyCard.DentalName,
                DentistName = warrantyCard.DentistName,
                LaboName = warrantyCard.LaboName,
                StartDate = warrantyCard.StartDate,
                Description = warrantyCard.Description,
                Image = warrantyCard.Image,
                LinkCategory = warrantyCard.LinkCategory,
                Status = EnumUtil.ParseEnum<WarrantyCardStatus>(warrantyCard.Status),
                OrderId = warrantyCard.OrderId
            };
        }
    }
}
