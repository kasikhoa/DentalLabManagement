using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<CreateOrderResponse> CreateNewOrder(CreateOrderRequest createOrderRequest)
        {
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Dental dental = await _unitOfWork.GetRepository<Dental>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(createOrderRequest.DentalId));
            if (dental == null) throw new HttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);          

            Order newOrder = new Order()
            {
                DentalId = dental.Id,
                DentistName = createOrderRequest.DentistName,
                DentistNote = createOrderRequest.DentistNote,
                PatientName = createOrderRequest.PatientName,
                PatientGender = createOrderRequest.PatientGender.GetDescriptionFromEnum(),               
                Status = createOrderRequest.Status.GetDescriptionFromEnum(),
                Mode = createOrderRequest.Mode.GetDescriptionFromEnum(),
                TotalAmount = createOrderRequest.TotalAmount,
                Discount = createOrderRequest.Discount,
                FinalAmount = createOrderRequest.TotalAmount - createOrderRequest.Discount,
                CreatedDate = currentTime,
            };
           
            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.CommitAsync();

            newOrder.InvoiceId = "E" + (dental.Id * 10000 + newOrder.Id).ToString("D5");
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
            return new CreateOrderResponse(newOrder.Id, newOrder.InvoiceId, dental.Name, newOrder.DentistName , newOrder.DentistNote, newOrder.PatientName, 
                EnumUtil.ParseEnum<PatientGender>(newOrder.PatientGender), EnumUtil.ParseEnum<OrderStatus>(newOrder.Status),
                EnumUtil.ParseEnum<OrderMode>(newOrder.Mode), newOrder.TeethQuantity, newOrder.TotalAmount, newOrder.Discount, newOrder.FinalAmount, newOrder.CreatedDate);

        }

        public async Task<IPaginate<GetOrderDetailResponse>> GetOrders(string? InvoiceId, OrderMode? mode, OrderStatus? status, int page, int size)
        {
            InvoiceId = InvoiceId?.Trim().ToLower();

            // Thực hiện truy vấn lấy danh sách đơn hàng với điều kiện lọc (status, mode) và phân trang (page, size).
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
                    Status = EnumUtil.ParseEnum<OrderStatus>(x.Status),
                    Mode = EnumUtil.ParseEnum<OrderMode>(x.Mode),
                    TeethQuantity = x.TeethQuantity,
                    TotalAmount = x.TotalAmount,
                    Discount = x.Discount,
                    FinalAmount = x.FinalAmount,
                    CreatedDate = x.CreatedDate
                },
                predicate: string.IsNullOrEmpty(InvoiceId) && (mode == null) && (status == null)
                ? x => true
                : ((status == null && mode == null)
                    ? x => x.InvoiceId.Contains(InvoiceId)
                    : ((status == null)
                        ? x => x.InvoiceId.Contains(InvoiceId) && x.Mode.Equals(mode.GetDescriptionFromEnum())
                        : x => x.InvoiceId.Contains(InvoiceId) && x.Mode.Equals(mode.GetDescriptionFromEnum()) 
                            && x.Status.Equals(status.GetDescriptionFromEnum()))),
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
                                Id = x.Id,
                                ToothArch = x.TeethPosition.ToothArch,
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


        public async Task<GetOrderDetailResponse> GetOrderTeethDetals(int id)
        {
            if (id < 1) throw new HttpRequestException(MessageConstant.Order.EmptyOrderIdMessage);
            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (order == null) throw new HttpRequestException(MessageConstant.Order.OrderNotFoundMessage);
            Dental dental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(order.DentalId));
            if (dental == null) throw new HttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            GetOrderDetailResponse orderItemResponse = new GetOrderDetailResponse();
            orderItemResponse.Id = order.Id;
            orderItemResponse.InvoiceId = order.InvoiceId;
            orderItemResponse.DentalName = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                selector: x => x.Name, predicate: x => x.Id.Equals(order.DentalId));
            orderItemResponse.DentistName = order.DentistName;
            orderItemResponse.DentistNote = order.DentistNote;
            orderItemResponse.PatientName = order.PatientName;
            orderItemResponse.PatientGender = EnumUtil.ParseEnum<PatientGender>(order.PatientGender);
            orderItemResponse.Status = EnumUtil.ParseEnum<OrderStatus>(order.Status);
            orderItemResponse.Mode = EnumUtil.ParseEnum<OrderMode>(order.Mode);
            orderItemResponse.TeethQuantity = order.TeethQuantity;
            orderItemResponse.TotalAmount = order.TotalAmount;
            orderItemResponse.Discount = order.Discount;
            orderItemResponse.FinalAmount = order.FinalAmount;
            orderItemResponse.CreatedDate = order.CreatedDate;


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
                            Id = x.Id,
                            ToothArch = x.TeethPosition.ToothArch,
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
    }
}
