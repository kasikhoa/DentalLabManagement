using AutoMapper;
using DentalLabManagement.API.Extensions;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Payload.OrderItem;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class OrderItemService : BaseService<OrderItemService>, IOrderItemService
    {
        public OrderItemService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderItemService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        private Expression<Func<OrderItem, bool>> BuildGetOrderItemsQuery(int? orderId, string? warrantyCardCode, OrderItemMode? mode)
        {
            Expression<Func<OrderItem, bool>> filterQuery = x => true; 

            if (orderId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.OrderId.Equals(orderId));
            }

            if (!string.IsNullOrEmpty(warrantyCardCode))
            {
                filterQuery = filterQuery.AndAlso(x => x.WarrantyCard.CardCode.Contains(warrantyCardCode));
            }

            if (mode != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Mode.Equals(mode.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<GetOrderItemResponse>> GetOrderItems(int? orderId, string? warrantyCardCode, OrderItemMode? mode, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<GetOrderItemResponse> result = await _unitOfWork.GetRepository<OrderItem>().GetPagingListAsync(
                selector: x => new GetOrderItemResponse()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ProductName = x.Product.Name,
                    TeethPosition = x.TeethPosition.PositionName,                   
                    Note = x.Note,
                    TotalAmount = x.TotalAmount,
                    WarrantyCardCode = x.WarrantyCard.CardCode,
                    Mode = EnumUtil.ParseEnum<OrderItemMode>(x.Mode), 
                },
                predicate: BuildGetOrderItemsQuery(orderId, warrantyCardCode, mode),
                page: page,
                size: size);
            return result;
        }

        public async Task<GetOrderItemResponse> GetOrderItemById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.OrderItem.EmptyIdMessage);
            OrderItem orderItem = await _unitOfWork.GetRepository<OrderItem>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Product).Include(x => x.TeethPosition).Include(x => x.WarrantyCard)
                );
            if (orderItem == null) throw new BadHttpRequestException(MessageConstant.OrderItem.NotFoundMessage);

            return new GetOrderItemResponse()
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductName = orderItem.Product.Name,
                TeethPosition = orderItem.TeethPosition.PositionName,             
                Note = orderItem.Note,
                TotalAmount = orderItem.TotalAmount,
                WarrantyCardCode = orderItem.WarrantyCard?.CardCode,
                Mode = EnumUtil.ParseEnum<OrderItemMode>(orderItem.Mode),
            };
        }

        public async Task<bool> UpdateOrderItem(int id, UpdateOrderItemRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.OrderItem.EmptyIdMessage);
            OrderItem orderItem = await _unitOfWork.GetRepository<OrderItem>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
                );
            if (orderItem == null) throw new BadHttpRequestException(MessageConstant.OrderItem.NotFoundMessage);

            Product product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.ProductId));
            if (product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFoundMessage);

            TeethPosition teethPosition = await _unitOfWork.GetRepository<TeethPosition>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.TeethPositionId));
            if (teethPosition == null) throw new BadHttpRequestException(MessageConstant.TeethPosition.IdNotFoundMessage);

            request.TrimString();

            orderItem.ProductId = request.ProductId;
            orderItem.TeethPositionId = request.TeethPositionId;
            orderItem.Note = string.IsNullOrEmpty(request.Note) ? orderItem.Note : request.Note;
            orderItem.TotalAmount = (request.TotalAmount < 0) ? orderItem.TotalAmount : request.TotalAmount;

            _unitOfWork.GetRepository<OrderItem>().UpdateAsync(orderItem);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> InsertWarrantyCard(int id, InsertWarrantyCardRequest updateRequest)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.OrderItem.EmptyIdMessage);

            OrderItem orderItem = await _unitOfWork.GetRepository<OrderItem>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Product)
                );
            if (orderItem == null) throw new BadHttpRequestException(MessageConstant.OrderItem.NotFoundMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderItem.OrderId)
                );
            if (!order.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()))
                throw new BadHttpRequestException(MessageConstant.Order.OrderNotCompletedMessage);               

            WarrantyCard warrantyCard = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateRequest.WarrantyCardId),
                include: x => x.Include(x => x.CardType)
                );
            if (warrantyCard == null) throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotFoundMessage);

            if (!warrantyCard.CardType.CategoryId.Equals(orderItem.Product.CategoryId))
                throw new BadHttpRequestException(MessageConstant.WarrantyCard.CardNotMatchedCategoryMessage);
           
            orderItem.WarrantyCardId = updateRequest.WarrantyCardId;
            warrantyCard.ExpDate = order.CompletedDate?.AddYears(warrantyCard.CardType.WarrantyYear);

            _unitOfWork.GetRepository<OrderItem>().UpdateAsync(orderItem);
            _unitOfWork.GetRepository<WarrantyCard>().UpdateAsync(warrantyCard);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateStatusToWarranty(int orderItemId, WarrantyOrderItemRequest request)
        {
            if (orderItemId < 1) throw new BadHttpRequestException(MessageConstant.OrderItem.EmptyIdMessage);

            OrderItem orderItem = await _unitOfWork.GetRepository<OrderItem>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderItemId),
                include: x => x.Include(x => x.Product)
                );
            if (orderItem == null) throw new BadHttpRequestException(MessageConstant.OrderItem.NotFoundMessage);

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            orderItem.Mode = OrderItemMode.Warranty.GetDescriptionFromEnum();
            orderItem.Note = string.IsNullOrEmpty(request.Note) ? orderItem.Note : request.Note;

            List<OrderItemStage> orderItemStageList = new List<OrderItemStage>();

            ICollection<GroupStage> stageList = await _unitOfWork.GetRepository<GroupStage>().GetListAsync(
                predicate: x => x.CategoryId.Equals(orderItem.Product.CategoryId),
                include: x => x.Include(x => x.ProductStage)
                );

            foreach (var itemStage in stageList)
            {
                OrderItemStage newStage = new OrderItemStage()
                {
                    OrderItemId = orderItemId,
                    StageId = itemStage.ProductStage.Id,
                    StartTime = currentTime,
                    ExpectedTime = currentTime.AddHours(itemStage.ProductStage.ExecutionTime),
                    Status = OrderItemStageStatus.Waiting.GetDescriptionFromEnum(),
                    Mode = OrderItemMode.Warranty.GetDescriptionFromEnum(),
                };
                orderItemStageList.Add(newStage);
            }
            _unitOfWork.GetRepository<OrderItem>().UpdateAsync(orderItem);
            await _unitOfWork.GetRepository<OrderItemStage>().InsertRangeAsync(orderItemStageList);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
