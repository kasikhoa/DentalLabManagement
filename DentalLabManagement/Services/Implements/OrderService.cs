﻿using DentalLabManagement.API.Extensions;
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
using DentalLabManagement.BusinessTier.Payload.OrderHistory;
using AutoMapper;

namespace DentalLabManagement.API.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest createOrderRequest)
        {

            Dental dental = await _unitOfWork.GetRepository<Dental>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(createOrderRequest.DentalId));
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            string currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.UserName.Equals(currentUser));

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Order newOrder = new Order()
            {
                DentalId = dental.Id,
                DentistName = createOrderRequest.DentistName,
                DentistNote = createOrderRequest.DentistNote,
                PatientName = createOrderRequest.PatientName,
                PatientGender = createOrderRequest.PatientGender.GetDescriptionFromEnum(),
                PatientPhoneNumber = string.IsNullOrEmpty(createOrderRequest.PatientPhoneNumber) ? null : createOrderRequest.PatientPhoneNumber,
                Status = OrderStatus.Pending.GetDescriptionFromEnum(),
                TotalAmount = createOrderRequest.TotalAmount,
                Discount = createOrderRequest.Discount,
                FinalAmount = createOrderRequest.TotalAmount - createOrderRequest.Discount,
                Note = createOrderRequest.Note,
                CreatedDate = currentTime,
            };

            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);            
            await _unitOfWork.CommitAsync();

            OrderHistory orderHistory = new OrderHistory()
            {
                OrderId = newOrder.Id,
                CreatedDate = currentTime,
                CreatedBy = account.Id,
                Note = newOrder.Note,
                Status = newOrder.Status,
                
            };
            await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(orderHistory);

            newOrder.InvoiceId = "E" + newOrder.Id.ToString("D6");
            int count = 0;

            List<OrderItem> orderItems = new List<OrderItem>();
            createOrderRequest.ProductsList.ForEach(product =>
            {
                orderItems.Add(new OrderItem()
                {
                    OrderId = newOrder.Id,
                    ProductId = product.ProductId,
                    TeethPositionId = product.TeethPositionId,
                    Note = product.Note,
                    TotalAmount = product.TotalAmount,
                    Mode = OrderItemMode.New.GetDescriptionFromEnum(),
                });
                count++;
            });

            newOrder.TeethQuantity = count;
            await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
            await _unitOfWork.CommitAsync();
            return new CreateOrderResponse(newOrder.Id, newOrder.InvoiceId, dental.Name,
                newOrder.DentistName, newOrder.DentistNote, newOrder.PatientName,
                EnumUtil.ParseEnum<PatientGender>(newOrder.PatientGender), newOrder.PatientPhoneNumber,
                EnumUtil.ParseEnum<OrderStatus>(newOrder.Status), newOrder.TeethQuantity,
                newOrder.TotalAmount, newOrder.Discount, newOrder.FinalAmount, newOrder.Note, newOrder.CreatedDate);

        }

        private Expression<Func<Order, bool>> BuildGetOrdersQuery(string? invoiceId, int? dentalId, string? dentistName, string? patientName, 
            string? patientPhoneNumber, OrderStatus? status, DateTime? createdDate, DateTime? completedDate)
        {
            Expression<Func<Order, bool>> filterQuery = p => true;

            if (!string.IsNullOrEmpty(invoiceId))
            {
                filterQuery = filterQuery.AndAlso(p => p.InvoiceId.Contains(invoiceId));
            }

            if (dentalId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(p => p.DentalId.Equals(dentalId));
            }

            if (!string.IsNullOrEmpty(dentistName))
            {
                filterQuery = filterQuery.AndAlso(p => p.DentistName.Contains(dentistName));
            }

            if (!string.IsNullOrEmpty(patientName))
            {
                filterQuery = filterQuery.AndAlso(p => p.PatientName.Contains(patientName));
            }

            if (!string.IsNullOrEmpty(patientPhoneNumber))
            {
                filterQuery = filterQuery.AndAlso(p => p.PatientPhoneNumber.Contains(patientPhoneNumber));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Status.Equals(status.GetDescriptionFromEnum()));
            }

            if (createdDate.HasValue)
            {
                filterQuery = filterQuery.AndAlso(p => p.CreatedDate >= createdDate);
            }

            if (completedDate.HasValue)
            {
                filterQuery = filterQuery.AndAlso(p => p.CompletedDate <= completedDate);
            }

            return filterQuery;
        }

        public async Task<IPaginate<GetOrdersResponse>> GetOrders(string? invoiceId, int? dentalId, string? dentistName, string? patientName, 
            string? patientPhoneNumber, OrderStatus? status, DateTime? createdDate, DateTime? completedDate, int page, int size)
        {
            invoiceId = invoiceId?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<GetOrdersResponse> orderList = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: x => new GetOrdersResponse()
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
                    TeethQuantity = x.TeethQuantity,
                    TotalAmount = x.TotalAmount,
                    Discount = x.Discount,
                    FinalAmount = x.FinalAmount,
                    CreatedDate = x.CreatedDate,
                    CompletedDate = x.CompletedDate,
                    Note = x.Note,
                },
                predicate: BuildGetOrdersQuery(invoiceId, dentalId, dentistName, patientName, patientPhoneNumber, status, createdDate, completedDate),
                orderBy: x => x.OrderBy(x => x.InvoiceId),
                page: page,
                size: size
            );
            return orderList;
        }


        public async Task<GetOrderDetailResponse> GetOrderTeethDetail(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);
            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Dental)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            GetOrderDetailResponse orderItemResponse = new GetOrderDetailResponse()
            {
                Id = order.Id,
                InvoiceId = order.InvoiceId,
                DentalName = order.Dental.Name,
                DentistName = order.DentistName,
                DentistNote = order.DentistNote,
                PatientName = order.PatientName,
                PatientGender = EnumUtil.ParseEnum<PatientGender>(order.PatientGender),
                PatientPhoneNumber = order.PatientPhoneNumber,
                Status = EnumUtil.ParseEnum<OrderStatus>(order.Status),
                TeethQuantity = order.TeethQuantity,
                TotalAmount = order.TotalAmount,
                Discount = order.Discount,
                FinalAmount = order.FinalAmount,
                CreatedDate = order.CreatedDate,
                CompletedDate = order.CompletedDate,
                Note = order.Note,
            };            

            orderItemResponse.ToothList = (List<OrderItemResponse>)await _unitOfWork.GetRepository<OrderItem>()
                .GetListAsync(
                    selector: x => new OrderItemResponse()
                    {
                        Id = x.Id,
                        Product = new ProductResponse()
                        {
                            Id = x.ProductId,
                            CategoryName = x.Product.Category.Name,
                            Name = x.Product.Name,
                            Description = x.Product.Description,
                            CostPrice = x.Product.CostPrice                           
                        },
                        TeethPosition = new TeethPositionResponse()
                        {
                            Id = x.TeethPositionId,
                            ToothArch = EnumUtil.ParseEnum<ToothArch>(x.TeethPosition.ToothArch.ToString()),
                            PositionName = x.TeethPosition.PositionName,
                            Description = x.TeethPosition.Description
                        },
                        Note = x.Note,
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
                        PaymentTime = x.PaymentTime,
                        Status = EnumUtil.ParseEnum<PaymentStatus>(x.Status),
                    },
                    predicate: x => x.OrderId.Equals(id)
                );

            return orderItemResponse;

        }

        public async Task<bool> UpdateOrderStatus(int orderId, UpdateOrderRequest updateOrderRequest)
        {
            if (orderId < 1) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);
            Order updateOrder = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId)
                );
            if (updateOrder == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            string currentUser = GetUsernameFromJwt();
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.UserName.Equals(currentUser)
                );

            ICollection<OrderItem> orderItems = await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                predicate: x => x.OrderId.Equals(orderId),
                include: x => x.Include(x => x.Product)
                );

            OrderHistory orderHistory;

            OrderStatus status = updateOrderRequest.Status;

            switch (status)
            {
                case OrderStatus.Warranty:
                    updateOrder.Status = updateOrderRequest.Status.GetDescriptionFromEnum();

                    orderHistory = new OrderHistory()
                    {
                        OrderId = orderId,
                        CreatedDate = currentTime,
                        CreatedBy = account.Id,
                        Note = updateOrder.Note,
                        Status = updateOrder.Status,
                    };
                    await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(orderHistory);
                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    break;

                case OrderStatus.Producing:
                    if (updateOrder.Status.Equals(OrderStatus.Producing.GetDescriptionFromEnum()))
                        throw new BadHttpRequestException(MessageConstant.Order.ProducingStatusRepeatMessage);

                    if (updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        throw new BadHttpRequestException(MessageConstant.Order.CanceledStatusRepeatMessage);

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
                                StageId = itemStage.ProductStage.Id,
                                StartTime = currentTime,
                                ExpectedTime = currentTime.AddHours(itemStage.ProductStage.ExecutionTime),
                                Status = OrderItemStageStatus.Waiting.GetDescriptionFromEnum(),                                                               
                                Mode = OrderItemMode.New.GetDescriptionFromEnum(),
                            };
                            orderItemStageList.Add(newStage);
                        }

                    }

                    await _unitOfWork.GetRepository<OrderItemStage>().InsertRangeAsync(orderItemStageList);

                    updateOrder.Status = updateOrderRequest.Status.GetDescriptionFromEnum();
                    updateOrder.Note = string.IsNullOrEmpty(updateOrderRequest.Note) ? updateOrder.Note : updateOrderRequest.Note;

                    orderHistory = new OrderHistory()
                    {
                        OrderId = orderId,
                        CreatedDate = currentTime,
                        CreatedBy = account.Id,
                        Note = updateOrderRequest.Note,
                        Status = updateOrder.Status,
                    };
                    await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(orderHistory);
                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    break;
                    
                case OrderStatus.Completed:
                    if (updateOrder.Status.Equals(OrderStatus.Canceled.GetDescriptionFromEnum()))
                        throw new BadHttpRequestException(MessageConstant.Order.CanceledStatusRepeatMessage);

                    List<int> orderItemIds = (List<int>) await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
                        selector: x => x.Id, predicate: x => x.OrderId.Equals(orderId));

                    ICollection<OrderItemStage> itemStageList = await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
                        predicate: x => orderItemIds.Contains(x.OrderItemId) && x.Mode.Equals(orderItems.FirstOrDefault().Mode));

                    bool allCompleted = itemStageList.All(itemStage => itemStage.Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()));

                    if (!allCompleted)
                    {
                        throw new BadHttpRequestException(MessageConstant.Order.UpdateFailedByStageMessage);
                    }

                    if (updateOrder.Status.Equals(OrderStatus.Warranty.GetDescriptionFromEnum()))
                    {

                        updateOrder.Status = updateOrderRequest.Status.GetDescriptionFromEnum();
                        updateOrder.Note = string.IsNullOrEmpty(updateOrderRequest.Note) ? updateOrder.Note : updateOrderRequest.Note;

                        orderHistory = new OrderHistory()
                        {
                            OrderId = orderId,
                            CreatedDate = currentTime,
                            CreatedBy = account.Id,
                            CompletedDate = currentTime,
                            Note = updateOrderRequest.Note,
                            Status = OrderHistoryStatus.CompletedWarranty.GetDescriptionFromEnum(),
                        };
                        await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(orderHistory);
                        _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                        break;
                    }

                    updateOrder.Status = updateOrderRequest.Status.GetDescriptionFromEnum();
                    updateOrder.CompletedDate = currentTime;
                    updateOrder.Note = string.IsNullOrEmpty(updateOrderRequest.Note) ? updateOrder.Note : updateOrderRequest.Note;

                    orderHistory = new OrderHistory()
                    {
                        OrderId = orderId,
                        CreatedDate = currentTime,
                        CreatedBy = account.Id,
                        CompletedDate = updateOrder.CompletedDate,
                        Note = updateOrderRequest.Note,
                        Status = updateOrder.Status,
                    };
                    await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(orderHistory);
                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    break;

                case OrderStatus.Canceled:
                    updateOrder.Status = OrderStatus.Canceled.GetDescriptionFromEnum();
                    updateOrder.Note = string.IsNullOrEmpty(updateOrderRequest.Note) ? updateOrder.Note : updateOrderRequest.Note;
                    orderHistory = new OrderHistory()
                    {
                        OrderId = orderId,
                        CreatedDate = currentTime,
                        CreatedBy = account.Id,
                        Note = updateOrder.Note,
                        Status = updateOrder.Status,
                    };
                    await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(orderHistory);

                    _unitOfWork.GetRepository<Order>().UpdateAsync(updateOrder);
                    break;

                default:
                    return false;

            }
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<PaymentResponse> UpdateOrderPayment(int orderId, PaymentRequest paymentRequest)
        {
            if (orderId < 1) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);
            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId));
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            Payment payment = await _unitOfWork.GetRepository<Payment>().SingleOrDefaultAsync(
                predicate: x => x.OrderId.Equals(orderId));
            if (payment != null) throw new BadHttpRequestException(MessageConstant.Order.OrderPaidFullMessage);        

            if (!order.Status.Equals(OrderStatus.Completed.GetDescriptionFromEnum()))
                throw new BadHttpRequestException(MessageConstant.Order.OrderNotCompletedMessage);

            if (paymentRequest.Amount != order.FinalAmount)
            {
                throw new BadHttpRequestException(MessageConstant.Order.AmountErrorMessage);
            }

            payment = new Payment()
            {               
                OrderId = orderId,
                Note = paymentRequest.Note,
                PaymentType = paymentRequest.Type.GetDescriptionFromEnum(),
                Amount = paymentRequest.Amount,
                PaymentTime = TimeUtils.GetCurrentSEATime(),
                Status = PaymentStatus.Success.GetDescriptionFromEnum()
            };
                     
            await _unitOfWork.GetRepository<Payment>().InsertAsync(payment);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Order.PaymentFailedMessage);

            return new PaymentResponse()
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Note = payment.Note,
                PaymentType = EnumUtil.ParseEnum<PaymentType>(payment.PaymentType),
                Amount = payment.Amount,
                PaymentTime = payment.PaymentTime,
                Status = EnumUtil.ParseEnum<PaymentStatus>(payment.Status),
            };
        }

        private Expression<Func<Payment, bool>> BuildGetPaymentsQuery(int orderId, PaymentType? type, PaymentStatus? status)
        {
            Expression<Func<Payment, bool>> filterQuery = p => p.OrderId.Equals(orderId);

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

        public async Task<IPaginate<PaymentResponse>> GetOrderPayments(int orderId, PaymentType? type, PaymentStatus? status, int page, int size)
        {

            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            IPaginate<PaymentResponse> result = await _unitOfWork.GetRepository<Payment>().GetPagingListAsync(
                selector: x => new PaymentResponse()
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    Note = x.Note,
                    PaymentType = EnumUtil.ParseEnum<PaymentType>(x.PaymentType),
                    Amount = x.Amount,
                    PaymentTime = x.PaymentTime,
                    Status = EnumUtil.ParseEnum<PaymentStatus>(x.Status)
                },
                predicate: BuildGetPaymentsQuery(orderId, type, status),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<OrderHistoryResponse> CreateWarrantyRequest(int orderId, CreateWarrantyRequest request)
        {
            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderId)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            string currentUser = GetUsernameFromJwt();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.UserName.Equals(currentUser)
                );

            OrderHistory warrantyRequest = new OrderHistory()
            {
                OrderId = orderId,
                CreatedDate = TimeUtils.GetCurrentSEATime(),
                CreatedBy = account.Id,                
                Note = request.Note,
                Status = OrderHistoryStatus.WarrantyRequest.GetDescriptionFromEnum()
            };

            await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(warrantyRequest);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.WarrantyRequest.WarrantyRequestFailedMessage);
            return new OrderHistoryResponse(warrantyRequest.CreatedDate, account.FullName, warrantyRequest.CompletedDate, 
                warrantyRequest.Note, EnumUtil.ParseEnum<OrderHistoryStatus>(warrantyRequest.Status));
        }

        private Expression<Func<OrderHistory, bool>> BuildGetOrderHistoryQuery(int orderId, OrderHistoryStatus? status)
        {
            Expression<Func<OrderHistory, bool>> filterQuery = p => p.OrderId.Equals(orderId);

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<OrderHistoryResponse>> ViewOrderHistory(int orderId, OrderHistoryStatus? status, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
               predicate: x => x.Id.Equals(orderId)
               );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            IPaginate<OrderHistoryResponse> result = await _unitOfWork.GetRepository<OrderHistory>().GetPagingListAsync(
                selector: x => new OrderHistoryResponse(x.CreatedDate, x.CreatedByNavigation.FullName, x.CompletedDate, 
                    x.Note, EnumUtil.ParseEnum<OrderHistoryStatus>(x.Status)),
                predicate: BuildGetOrderHistoryQuery(orderId, status),
                page: page,
                size: size
                );
            return result;
        }
    }
}
