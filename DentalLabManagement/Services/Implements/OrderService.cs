using DentalLabManagement.API.Extensions;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DentalLabManagement.API.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest createOrderRequest)
        {
            Dental dental = await _unitOfWork.GetRepository<Dental>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(createOrderRequest.DentalId));
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            Order newOrder = new Order()
            {
                DentalId = dental.Id,
                DentistName = createOrderRequest.DentistName,
                DentistNote = createOrderRequest.DentistNote,
                PatientName = createOrderRequest.PatientName,
                PatientGender = createOrderRequest.PatientGender.GetDescriptionFromEnum(),
                PatientPhoneNumber= createOrderRequest.PatientPhoneNumber,
                Status = OrderStatus.New.GetDescriptionFromEnum(),
                Mode = createOrderRequest.Mode.GetDescriptionFromEnum(),
                TotalAmount = createOrderRequest.TotalAmount,
                Discount = createOrderRequest.Discount,
                FinalAmount = createOrderRequest.TotalAmount - createOrderRequest.Discount,
                CreatedDate = TimeUtils.GetCurrentSEATime(),
            };

            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.CommitAsync();

            newOrder.InvoiceId = "E" + newOrder.Id.ToString("D6");
            
            await _unitOfWork.CommitAsync();
            int count = 0;

            List<OrderItem> orderItems = new List<OrderItem>();
            createOrderRequest.ProductsList.ForEach(product =>
            {
                double totalProductAmount = product.SellingPrice * product.Quantity;
                orderItems.Add(new OrderItem()
                {
                    OrderId = newOrder.Id,
                    ProductId = product.ProductId,
                    TeethPositionId = product.TeethPositionId,
                    SellingPrice = product.SellingPrice,
                    Quantity = product.Quantity,
                    Note = product.Note,
                    TotalAmount = totalProductAmount,
                });
                count++;
            });

            newOrder.TeethQuantity = count;
            await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
            await _unitOfWork.CommitAsync();
            return new CreateOrderResponse(newOrder.Id, newOrder.InvoiceId, dental.Name,
                newOrder.DentistName, newOrder.DentistNote, newOrder.PatientName,
                EnumUtil.ParseEnum<PatientGender>(newOrder.PatientGender), newOrder.PatientPhoneNumber,
                EnumUtil.ParseEnum<OrderStatus>(newOrder.Status),
                EnumUtil.ParseEnum<OrderMode>(newOrder.Mode), newOrder.TeethQuantity,
                newOrder.TotalAmount, newOrder.Discount, newOrder.FinalAmount, newOrder.CreatedDate);

        }

        private Expression<Func<Order, bool>> BuildGetOrdersQuery(string? InvoiceId, OrderMode? mode, OrderStatus? status)
        {
            Expression<Func<Order, bool>> filterQuery = p => true; 

            if (!string.IsNullOrEmpty(InvoiceId))
            {
                filterQuery = filterQuery.AndAlso(p => p.InvoiceId.Contains(InvoiceId));
            }

            if (mode != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Mode.Equals(mode.GetDescriptionFromEnum()));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }


        public async Task<IPaginate<GetOrderDetailResponse>> GetOrders(string? InvoiceId, 
            OrderMode? mode, OrderStatus? status, int page, int size)
        {
            InvoiceId = InvoiceId?.Trim().ToLower();
            page = (page == 0) ? page = 1 : page;
            size = (size == 0) ? size = 10 : size;
            var orderList = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: x => new GetOrderDetailResponse()
                {
                    Id = x.Id,
                    InvoiceId = x.InvoiceId,
                    DentalName = x.Dental.Name,
                    DentistName = x.DentistName,
                    DentistNote = x.DentistNote,
                    PatientName = x.PatientName,
                    PatientGender = EnumUtil.ParseEnum<PatientGender>(x.PatientGender),
                    PatientPhoneNumber = x.PatientPhoneNumber,
                    Status = EnumUtil.ParseEnum<OrderStatus>(x.Status),
                    Mode = EnumUtil.ParseEnum<OrderMode>(x.Mode),
                    TeethQuantity = x.TeethQuantity,
                    TotalAmount = x.TotalAmount,
                    Discount = x.Discount,
                    FinalAmount = x.FinalAmount,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedByNavigation.FullName,
                },
                predicate: BuildGetOrdersQuery(InvoiceId, mode, status),
                orderBy: x => x.OrderBy(x => x.InvoiceId),
                page: page,
                size: size
            );
            
            foreach (var order in orderList.Items)
            {
                order.ToothList = (List<OrderItemResponse>) await _unitOfWork.GetRepository<OrderItem>()
                    .GetListAsync(
                        selector: x => new OrderItemResponse()
                        {
                            Id = x.Id,
                            OrderId = x.OrderId,
                            Product = new ProductResponse()
                            {
                                Id = x.ProductId,
                                Name = x.Product.Name,
                                Description = x.Product.Description,
                                CostPrice = x.Product.CostPrice,
                                CategoryId = x.Product.CategoryId
                            },
                            TeethPosition = new TeethPositionResponse()
                            {
                                Id = x.TeethPositionId,
                                ToothArch = EnumUtil.ParseEnum<ToothArch>(x.TeethPosition.ToothArch.ToString()),
                                PositionName = x.TeethPosition.PositionName,
                                Description = x.TeethPosition.Description
                            },
                            Note = x.Note,
                            SellingPrice = x.SellingPrice,
                            Quantity = x.Quantity,
                            TotalAmount = x.TotalAmount
                        },
                        predicate: x => x.OrderId.Equals(order.Id)
                    );

            }

            return orderList;
        }


        public async Task<GetOrderDetailResponse> GetOrderTeethDetail(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.UpdatedByNavigation).Include(x => x.Dental));
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            Dental dental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(order.DentalId));
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            GetOrderDetailResponse orderItemResponse = new GetOrderDetailResponse();
            orderItemResponse.Id = order.Id;
            orderItemResponse.InvoiceId = order.InvoiceId;
            orderItemResponse.DentalName = order.Dental.Name;
            orderItemResponse.DentistName = order.DentistName;
            orderItemResponse.DentistNote = order.DentistNote;
            orderItemResponse.PatientName = order.PatientName;
            orderItemResponse.PatientGender = EnumUtil.ParseEnum<PatientGender>(order.PatientGender);
            orderItemResponse.PatientPhoneNumber = order.PatientPhoneNumber;
            orderItemResponse.Status = EnumUtil.ParseEnum<OrderStatus>(order.Status);
            orderItemResponse.Mode = EnumUtil.ParseEnum<OrderMode>(order.Mode);
            orderItemResponse.TeethQuantity = order.TeethQuantity;
            orderItemResponse.TotalAmount = order.TotalAmount;
            orderItemResponse.Discount = order.Discount;
            orderItemResponse.FinalAmount = order.FinalAmount;
            orderItemResponse.CreatedDate = order.CreatedDate;
            orderItemResponse.UpdatedBy = order.UpdatedByNavigation.FullName;


            orderItemResponse.ToothList = (List<OrderItemResponse>) await _unitOfWork.GetRepository<OrderItem>()
                .GetListAsync(
                    selector: x => new OrderItemResponse()
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        Product = new ProductResponse()
                        {
                            Id = x.ProductId,
                            Name = x.Product.Name,
                            Description = x.Product.Description,
                            CostPrice = x.Product.CostPrice,
                            CategoryId = x.Product.CategoryId
                        },
                        TeethPosition = new TeethPositionResponse()
                        {
                            Id = x.TeethPositionId,
                            ToothArch = EnumUtil.ParseEnum<ToothArch>(x.TeethPosition.ToothArch.ToString()),
                            PositionName = x.TeethPosition.PositionName,
                            Description = x.TeethPosition.Description
                        },
                        Note = x.Note,
                        SellingPrice = x.SellingPrice,
                        Quantity = x.Quantity,
                        TotalAmount = x.TotalAmount
                    },
                    predicate: x => x.OrderId.Equals(id)
                );

            return orderItemResponse;

        }

        public async Task<UpdateOrderResponse> UpdateOrder(int orderId, UpdateOrderRequest updateOrderRequest)
        {
            if (orderId < 1) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateOrderRequest.UpdatedBy)
                );

            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);          

            Order updateOrder = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId),
                include: x => x.Include(x => x.UpdatedByNavigation)
                );
            if (updateOrder == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            ICollection<OrderItem> orderItems = await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                predicate: x => x.OrderId.Equals(orderId),
                include: x => x.Include(x => x.Product)
                );

            OrderStatus status = updateOrderRequest.Status;

            switch (status)
            {
                case OrderStatus.Producing:

                    if (updateOrder.Status.Equals(OrderStatus.Producing.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),updateOrder.UpdatedByNavigation.FullName, 
                            updateOrder.UpdatedAt, MessageConstant.Order.ProducingStatusRepeatMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()) ||
                        updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.UpdatedByNavigation.FullName,
                            updateOrder.UpdatedAt, MessageConstant.Order.CannotChangeToStatusMessage);

                    List<OrderItemStage> orderItemStageList = new List<OrderItemStage>();
                    foreach (var item in orderItems)
                    {
                        ICollection<GroupStage> stageList = await _unitOfWork.GetRepository<GroupStage>().GetListAsync(
                            predicate: p => p.CategoryId.Equals(item.Product.CategoryId),
                            include: x => x.Include(x => x.ProductStage)
                            );
                        foreach (var itemStage in stageList)
                        {
                            OrderItemStage newStage = new OrderItemStage()
                            {
                                OrderItemId = item.Id,
                                IndexStage = itemStage.ProductStage.IndexStage,
                                StageName = itemStage.ProductStage.Name,
                                Description = itemStage.ProductStage.Description,
                                ExecutionTime = itemStage.ProductStage.ExecutionTime,
                                Status = OrderItemStageStatus.Waiting.GetDescriptionFromEnum(),
                                StartDate = currentTime,
                            };
                            orderItemStageList.Add(newStage);
                        }

                    }
                    await _unitOfWork.GetRepository<OrderItemStage>().InsertRangeAsync(orderItemStageList);

                    updateOrder.Status = OrderStatus.Producing.GetDescriptionFromEnum();
                    updateOrder.UpdatedBy = updateOrderRequest.UpdatedBy;
                    updateOrder.UpdatedAt = currentTime;
                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);

                    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.UpdateStatusFailedMessage);

                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                        updateOrder.UpdatedByNavigation.FullName, updateOrder.UpdatedAt, MessageConstant.Order.ProducingStatusMessage);

                case OrderStatus.Completed:

                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                            await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        selector: x => x.FullName, predicate: x => x.Id.Equals(updateOrder.UpdatedBy)), 
                            updateOrder.UpdatedAt, MessageConstant.Order.CompletedStatusRepeatMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                            await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        selector: x => x.FullName, predicate: x => x.Id.Equals(updateOrder.UpdatedBy)),
                            updateOrder.UpdatedAt, MessageConstant.Order.CanceledStatusRepeatMessage);

                    foreach (var orderItem in orderItems)
                    {
                        ICollection<OrderItemStage> itemStageList = await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
                        predicate: x => x.OrderItemId.Equals(orderItem.Id));
                        foreach (var stage in itemStageList)
                        {
                            if (stage.Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()))
                            {
                                updateOrder.Status = updateOrderRequest.Status.GetDescriptionFromEnum();
                                updateOrder.UpdatedBy = updateOrderRequest.UpdatedBy;
                                updateOrder.UpdatedAt = currentTime;
                            }
                            
                            else return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                                account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.UpdateStatusFailedByStageMessage);
                        }
                    }                                                      

                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.UpdateStatusFailedMessage);

                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                        account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CompletedStatusMessage);  
                    
                case OrderStatus.Canceled:
                    if (updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                            await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                            selector: x => x.FullName, predicate: x => x.Id.Equals(updateOrder.UpdatedBy)), 
                            updateOrder.UpdatedAt, MessageConstant.Order.CanceledStatusRepeatMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                            await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                            selector: x => x.FullName, predicate: x => x.Id.Equals(updateOrder.UpdatedBy)), 
                            updateOrder.UpdatedAt, MessageConstant.Order.CompletedStatusRepeatMessage);

                    updateOrder.Status = OrderStatus.Canceled.GetDescriptionFromEnum();
                    updateOrder.UpdatedBy = updateOrderRequest.UpdatedBy;
                    updateOrder.UpdatedAt = currentTime;

                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.UpdateStatusFailedMessage);
                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                        updateOrder.UpdatedByNavigation.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CanceledStatusMessage);
                    
                default:
                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()) ||
                        updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.UpdatedByNavigation.FullName,
                            updateOrder.UpdatedAt, MessageConstant.Order.CannotChangeToStatusMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Producing.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.UpdatedByNavigation.FullName,
                            updateOrder.UpdatedAt, MessageConstant.Order.StatusErrorMessage);

                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status),
                        account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.NewStatusMessage);
                    
            }
        }
    }
}
