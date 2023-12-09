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

        public async Task<int> CreateNewOrder(CreateOrderRequest createOrderRequest)
        {
            Partner dental = await _unitOfWork.GetRepository<Partner>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(createOrderRequest.DentalId)
                );
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            string currentUser = GetUsernameFromJwt();
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.UserName.Equals(currentUser));

            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Order newOrder = new Order()
            {
                PartnerId = dental.Id,
                DentistName = createOrderRequest.DentistName,
                DentistNote = createOrderRequest.DentistNote,
                PatientName = createOrderRequest.PatientName,
                PatientGender = createOrderRequest.PatientGender.GetDescriptionFromEnum(),
                PatientPhoneNumber = createOrderRequest.PatientPhoneNumber,
                Status = OrderStatus.Pending.GetDescriptionFromEnum(),
                TotalAmount = createOrderRequest.TotalAmount,
                Discount = createOrderRequest.Discount,
                FinalAmount = createOrderRequest.TotalAmount - createOrderRequest.Discount,
                Note = createOrderRequest.Note,
                CreatedDate = currentTime,
            };

            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.CommitAsync();
            newOrder.InvoiceId = NameConstant.Company.Name + newOrder.Id.ToString("D6");

            OrderHistory orderHistory = new OrderHistory()
            {
                OrderId = newOrder.Id,
                CreatedDate = currentTime,
                CreatedBy = account.Id,
                Note = newOrder.Note,
                Status = newOrder.Status,

            };
            await _unitOfWork.GetRepository<OrderHistory>().InsertAsync(orderHistory);

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

                if (product.Extras.Count > 0)
                {
                    product.Extras.ForEach(extra =>
                    {
                        orderItems.Add(new OrderItem()
                        {
                            OrderId = newOrder.Id,
                            ProductId = extra.ProductId,
                            TeethPositionId = product.TeethPositionId,
                            Note = extra.Note,
                            TotalAmount = extra.TotalAmount,
                            Mode = OrderItemMode.New.GetDescriptionFromEnum()
                        });
                    });
                }
            });

            await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
            await _unitOfWork.CommitAsync();

            return newOrder.Id;
        }

        private static Expression<Func<Order, bool>> BuildGetOrdersQuery(OrderFilter filter)
        {
            Expression<Func<Order, bool>> filterQuery = p => true;

            var invoiceId = filter.invoiceId;
            var dentalId = filter.dentalId;
            var dentistName = filter.dentistName;
            var patientName = filter.patientName;
            var patientPhoneNumber = filter.patientPhoneNumber;
            var status = filter.status;
            var startDate = filter.createdDateFrom;
            var endDate = filter.createdDateTo;

            if (!string.IsNullOrEmpty(invoiceId))
            {
                filterQuery = filterQuery.AndAlso(p => p.InvoiceId.Contains(invoiceId));
            }

            if (dentalId != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.PartnerId.Equals(dentalId));
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

            if (startDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.CreatedDate >= startDate);
            }

            if (endDate != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.CreatedDate <= endDate);
            }

            return filterQuery;
        }

        public async Task<IPaginate<GetOrdersResponse>> GetOrders(OrderFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            //RoleEnum userRole = EnumUtil.ParseEnum<RoleEnum>(GetRoleFromJwt());
            IPaginate<GetOrdersResponse> orderList = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: x => new GetOrdersResponse()
                {
                    Id = x.Id,
                    InvoiceId = x.InvoiceId,
                    DentalId = x.PartnerId,
                    DentistName = x.DentistName,
                    DentistNote = x.DentistNote,
                    PatientName = x.PatientName,
                    PatientGender = EnumUtil.ParseEnum<PatientGender>(x.PatientGender),
                    PatientPhoneNumber = x.PatientPhoneNumber,
                    Status = EnumUtil.ParseEnum<OrderStatus>(x.Status),
                    TeethQuantity = x.OrderItems.Select(x => x.TeethPositionId).Distinct().Count(),
                    TotalAmount = x.TotalAmount,
                    Discount = x.Discount,
                    FinalAmount = x.FinalAmount,
                    CreatedDate = x.CreatedDate,
                    CompletedDate = x.CompletedDate,
                    Note = x.Note,
                },
                //filter: filter,
                predicate: BuildGetOrdersQuery(filter),
                //orderBy: x => (userRole == RoleEnum.Reception) ? x.OrderByDescending(x => x.CreatedDate) : x.OrderByDescending(x => x.InvoiceId),
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
                include: x => x.Include(x => x.Partner).Include(x => x.OrderItems)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.OrderNotFoundMessage);

            GetOrderDetailResponse orderItemResponse = new GetOrderDetailResponse()
            {
                Id = order.Id,
                InvoiceId = order.InvoiceId,
                DentalName = order.Partner.Name,
                DentistName = order.DentistName,
                DentistNote = order.DentistNote,
                PatientName = order.PatientName,
                PatientGender = EnumUtil.ParseEnum<PatientGender>(order.PatientGender),
                PatientPhoneNumber = order.PatientPhoneNumber,
                Status = EnumUtil.ParseEnum<OrderStatus>(order.Status),
                TeethQuantity = order.OrderItems.Select(x => x.TeethPositionId).Distinct().Count(),
                TotalAmount = order.TotalAmount,
                Discount = order.Discount,
                FinalAmount = order.FinalAmount,
                CreatedDate = order.CreatedDate,
                CompletedDate = order.CompletedDate,
                Note = order.Note,

                ToothList = (List<OrderTeethResponse>)await _unitOfWork.GetRepository<OrderItem>()
                .GetListAsync(
                    selector: x => new OrderTeethResponse()
                    {
                        OrderTeethId = x.Id,
                        ProductName = x.Product.Name,
                        TeethPosition = x.TeethPosition.PositionName,
                        Note = x.Note,
                        TotalAmount = x.TotalAmount
                    },
                    predicate: x => x.OrderId.Equals(id)
                ),

                PaymentList = (List<PaymentResponse>)await _unitOfWork.GetRepository<Payment>()
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
                )
            };
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

                    List<OrderItemStage> listOrderItemStage = new List<OrderItemStage>();

                    foreach (var item in orderItems)
                    {
                        ICollection<ProductStageMapping> stageList = await _unitOfWork.GetRepository<ProductStageMapping>().GetListAsync(
                            predicate: p => p.ProductId.Equals(item.Product.Id),
                            include: x => x.Include(x => x.Stage)
                            );

                        foreach (var itemStage in stageList)
                        {
                            OrderItemStage newStage = new OrderItemStage()
                            {
                                OrderItemId = item.Id,
                                StageId = itemStage.Stage.Id,
                                IndexStage = itemStage.IndexStage,
                                StartTime = currentTime,
                                ExpectedTime = currentTime.AddHours(itemStage.Stage.ExecutionTime),
                                Status = OrderItemStageStatus.Waiting.GetDescriptionFromEnum(),
                                Mode = OrderItemMode.New.GetDescriptionFromEnum(),
                            };
                            listOrderItemStage.Add(newStage);
                        }

                    }

                    await _unitOfWork.GetRepository<OrderItemStage>().InsertRangeAsync(listOrderItemStage);

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

                    List<int> orderItemIds = (List<int>)await _unitOfWork.GetRepository<OrderItem>().GetListAsync(
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

        private Expression<Func<Payment, bool>> BuildGetPaymentsQuery(int orderId, PaymentFilter filter)
        {
            Expression<Func<Payment, bool>> filterQuery = p => p.OrderId.Equals(orderId);

            var type = filter.paymentType;
            var status = filter.status;

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

        public async Task<IPaginate<PaymentResponse>> GetOrderPayments(int orderId, PaymentFilter filter, int page, int size)
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
                predicate: BuildGetPaymentsQuery(orderId, filter),
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

        private static Expression<Func<OrderHistory, bool>> BuildGetOrderHistoryQuery(int orderId, OrderHistoryFilter filter)
        {
            Expression<Func<OrderHistory, bool>> filterQuery = p => p.OrderId.Equals(orderId);

            var status = filter.status;

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<OrderHistoryResponse>> ViewOrderHistory(int orderId, OrderHistoryFilter filter, int page, int size)
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
                predicate: BuildGetOrderHistoryQuery(orderId, filter),
                page: page,
                size: size
                );
            return result;
        }
    }
}
