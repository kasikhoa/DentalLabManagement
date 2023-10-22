using DentalLabManagement.API.Extensions;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Payload.OrderItem;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class OrderItemService : BaseService<OrderItemService>, IOrderItemService
    {
        public OrderItemService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderItemService> logger) : base(unitOfWork, logger)
        {

        }

        private Expression<Func<OrderItem, bool>> BuildGetOrderItemsQuery(int? orderId, string? warrantyCardCode)
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

            return filterQuery;
        }

        public async Task<IPaginate<GetOrderItemResponse>> GetOrderItems(int? orderId, string? warrantyCardCode, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<GetOrderItemResponse> result = await _unitOfWork.GetRepository<OrderItem>().GetPagingListAsync(
                selector: x => new GetOrderItemResponse()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    ProductName= x.Product.Name,
                    TeethPosition = x.TeethPosition.PositionName,
                    WarrantyCardCode = x.WarrantyCard.CardCode,
                    Note = x.Note,
                    SellingPrice = x.SellingPrice,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount,

                },
                predicate: BuildGetOrderItemsQuery(orderId, warrantyCardCode),
                page: page,
                size: size);
            return result;
        }

        public async Task<GetOrderItemResponse> UpdateOrderItem(int id, UpdateOrderItemRequest request)
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
            orderItem.SellingPrice = (request.SellingPrice < 0) ? orderItem.SellingPrice : request.SellingPrice;
            orderItem.TotalAmount = orderItem.SellingPrice * orderItem.Quantity;

            _unitOfWork.GetRepository<OrderItem>().UpdateAsync(orderItem);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.OrderItem.UpdateFailedMessage);

            return new GetOrderItemResponse()
            {
                Id = orderItem.Id,
                OrderId = orderItem.Id,
                ProductName = product.Name,
                TeethPosition = teethPosition.PositionName,
                //WarrantyCardCode = await _unitOfWork.GetRepository<WarrantyCard>().SingleOrDefaultAsync(
                //    selector: x => x.CardCode, predicate: x => x.Id.Equals(orderItem.WarrantyCardId)),
                Note = orderItem.Note,
                SellingPrice = orderItem.SellingPrice,
                Quantity = orderItem.Quantity,
                TotalAmount = orderItem.TotalAmount,
            };
        }
    }
}
