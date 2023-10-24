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
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DentalLabManagement.BusinessTier.Payload.Payment;

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
                PaymentStatus = OrderPaymentStatus.Pending.GetDescriptionFromEnum(),
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

        private Expression<Func<Order, bool>> BuildGetOrdersQuery(string? InvoiceId, OrderMode? mode, OrderStatus? status, OrderPaymentStatus? paymentStatus)
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

            if (paymentStatus != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.PaymentStatus.Equals(paymentStatus.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }


        public async Task<IPaginate<GetOrderDetailResponse>> GetOrders(string? InvoiceId, OrderMode? mode, OrderStatus? status, 
            OrderPaymentStatus? paymentStatus, int page, int size)
        {
            InvoiceId = InvoiceId?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

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
                    CompletedDate = x.CompletedDate,
                    UpdatedBy = x.UpdatedByNavigation.FullName,
                    Note = x.Note,
                    PaymentStatus = EnumUtil.ParseEnum<OrderPaymentStatus>(x.PaymentStatus),
                },
                predicate: BuildGetOrdersQuery(InvoiceId, mode, status, paymentStatus),
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
                order.PaymentList = (List<PaymentResponse>) await _unitOfWork.GetRepository<Payment>()
                    .GetListAsync(
                        selector: x => new PaymentResponse()
                        {
                            Id= x.Id,
                            OrderId = x.OrderId,
                            Note = x.Note,
                            PaymentType = EnumUtil.ParseEnum<PaymentType>(x.PaymentType),
                            Amount = x.Amount,
                            Remaining = x.RestAmount,
                            PaymentTime = x.PaymentTime,
                            Status = EnumUtil.ParseEnum<PaymentStatus>(x.Status),
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

            string updateBy = (order.UpdatedByNavigation != null) ? order.UpdatedByNavigation.FullName : null;

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
            orderItemResponse.UpdatedBy = updateBy;
            orderItemResponse.Note = order.Note;
            orderItemResponse.PaymentStatus = EnumUtil.ParseEnum<OrderPaymentStatus>(order.PaymentStatus);

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

            orderItemResponse.PaymentList = (List<PaymentResponse>) await _unitOfWork.GetRepository<Payment>()
                .GetListAsync(
                    selector: x => new PaymentResponse()
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        Note = x.Note,
                        PaymentType = EnumUtil.ParseEnum<PaymentType>(x.PaymentType),
                        Amount = x.Amount,
                        Remaining = x.RestAmount,
                        PaymentTime = x.PaymentTime,
                        Status = EnumUtil.ParseEnum<PaymentStatus>(x.Status), 
                    },
                    predicate: x => x.OrderId.Equals(id)
                    );

            return orderItemResponse;

        }

        public async Task<UpdateOrderResponse> UpdateOrderStatus(int orderId, UpdateOrderRequest updateOrderRequest)
        {
            if (orderId < 1) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);

            Order updateOrder = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId)
                );
            if (updateOrder == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateOrderRequest.UpdatedBy)
                );
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);                   

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
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate, account.FullName, 
                            updateOrder.UpdatedAt, MessageConstant.Order.ProducingStatusRepeatMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()) ||
                        updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate, account.FullName,
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

                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate, 
                        account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.ProducingStatusMessage);

                case OrderStatus.Completed:

                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate,
                            account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CompletedStatusRepeatMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate, 
                            account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CanceledStatusRepeatMessage);


                    List<int> orderItemIds = (List<int>) await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                        selector: x => x.Id, predicate: x => x.OrderId.Equals(orderId));

                    ICollection<OrderItemStage> itemStageList = await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
                       predicate: x => orderItemIds.Contains(x.OrderItemId));

                    bool allCompleted = itemStageList.All(itemStage => itemStage.Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()));

                    if (!allCompleted)
                    {
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate,
                            account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.UpdateStatusFailedByStageMessage);
                    }

                    updateOrder.Status = OrderStatus.Completed.GetDescriptionFromEnum();
                    updateOrder.UpdatedBy = updateOrderRequest.UpdatedBy;
                    updateOrder.UpdatedAt = currentTime; 
                    updateOrder.CompletedDate = currentTime;

                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.UpdateStatusFailedMessage);

                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate, 
                        account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CompletedStatusMessage);

                case OrderStatus.Canceled:
                    if (updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate,
                            account.FullName, updateOrder.UpdatedAt, updateOrder.Note, MessageConstant.Order.CanceledStatusRepeatMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate,
                            account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CompletedStatusRepeatMessage);

                    updateOrder.Status = OrderStatus.Canceled.GetDescriptionFromEnum();
                    updateOrder.UpdatedBy = updateOrderRequest.UpdatedBy;
                    updateOrder.UpdatedAt = currentTime;
                    updateOrder.Note = updateOrderRequest.Note;

                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.UpdateStatusFailedMessage);
                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate,
                        account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CanceledStatusMessage);
                    
                default:
                    if (updateOrder.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()) ||
                        updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate,
                            account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.CannotChangeToStatusMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Producing.GetDescriptionFromEnum()))
                        return new UpdateOrderResponse(
                            EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), updateOrder.CompletedDate,
                            account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.StatusErrorMessage);

                    return new UpdateOrderResponse(EnumUtil.ParseEnum<OrderStatus>(updateOrder.Status), 
                        updateOrder.CompletedDate, account.FullName, updateOrder.UpdatedAt, MessageConstant.Order.NewStatusMessage);
                    
            }
        }

        public async Task<PaymentResponse> UpdateOrderPayment(int orderId, PaymentRequest paymentRequest)
        {
            if (orderId < 1) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId));
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);          
            
            ICollection<Payment> prevPayments = await _unitOfWork.GetRepository<Payment>().GetListAsync(
               predicate: x => x.OrderId.Equals(orderId));

            var totalPreviousPayments = prevPayments.Sum(p => p.Amount);

            if (totalPreviousPayments >= order.FinalAmount)           
                throw new BadHttpRequestException(MessageConstant.Order.OrderPaidFullMessage);

            Payment payment = new Payment()
            {
                OrderId = orderId,
                Note = paymentRequest.Note,
                PaymentType = paymentRequest.Type.GetDescriptionFromEnum(),
                Amount = paymentRequest.Amount,
                PaymentTime = TimeUtils.GetCurrentSEATime(),
                Status = PaymentStatus.Success.GetDescriptionFromEnum(),
            };
            
            await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);           
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.PaymentFailedMessage);

            var restAmount = order.FinalAmount - totalPreviousPayments - paymentRequest.Amount;
            payment.RestAmount= restAmount;
            _unitOfWork.GetRepository<Payment>().UpdateAsync(payment);
            await _unitOfWork.CommitAsync();
            if (restAmount <= 0)
            {
                order.PaymentStatus = OrderPaymentStatus.Paid.GetDescriptionFromEnum();
                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                order.PaymentStatus = OrderPaymentStatus.Partial.GetDescriptionFromEnum();
                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                await _unitOfWork.CommitAsync();
            }
            return new PaymentResponse()
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Note = payment.Note,
                PaymentType = EnumUtil.ParseEnum<PaymentType>(payment.PaymentType),
                Amount = payment.Amount,
                Remaining = restAmount,
                PaymentTime = payment.PaymentTime,
                Status = EnumUtil.ParseEnum<PaymentStatus>(payment.Status),
            };
        }

        private Expression<Func<Payment, bool>> BuildGetPaymentsQuery(int? orderId, PaymentType? type, PaymentStatus? status)
        {
            Expression<Func<Payment, bool>> filterQuery = p => true;

            if (orderId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(p => p.OrderId.Equals(orderId));
            }

            if (type != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.PaymentType.Equals(type.GetDescriptionFromEnum()));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<PaymentResponse>> GetOrderPayments(int? orderId, PaymentType? type, PaymentStatus? status, int page, int size)
        {

            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<PaymentResponse> result = await _unitOfWork.GetRepository<Payment>().GetPagingListAsync(
                selector: x => new PaymentResponse()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    Note = x.Note,
                    PaymentType = EnumUtil.ParseEnum<PaymentType>(x.PaymentType),
                    Amount = x.Amount,
                    Remaining = x.RestAmount,
                    PaymentTime = x.PaymentTime,
                    Status = EnumUtil.ParseEnum<PaymentStatus>(x.Status)
                },
                predicate: BuildGetPaymentsQuery(orderId, type, status),
                page: page,
                size: size       
                );
            return result;
        }
    }
}
