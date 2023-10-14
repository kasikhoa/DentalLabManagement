using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
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
                PatientGender = createOrderRequest.PatientGender,               
                Status= createOrderRequest.Status.GetDescriptionFromEnum(),
                Mode = createOrderRequest.Mode.GetDescriptionFromEnum(),
                TotalAmount = createOrderRequest.TotalAmount,
                Discount = createOrderRequest.Discount,
                FinalAmount = createOrderRequest.TotalAmount - createOrderRequest.Discount,
                CreatedDate = currentTime,
            };
           
            await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
            await _unitOfWork.CommitAsync();

            string newInvoice = "E" + (dental.Id * 1000 + newOrder.Id).ToString("D4");
            newOrder.InvoiceId = newInvoice;
            await _unitOfWork.CommitAsync();

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
            });
           
            await _unitOfWork.GetRepository<OrderItem>().InsertRangeAsync(orderItems);
            await _unitOfWork.CommitAsync();
            return new CreateOrderResponse(newOrder.Id, newInvoice, dental.Name, newOrder.DentistName , newOrder.DentistNote, newOrder.PatientName, EnumUtil.ParseEnum<OrderStatus>(newOrder.Status),
                EnumUtil.ParseEnum<OrderMode>(newOrder.Mode), newOrder.TotalAmount, newOrder.Discount, newOrder.FinalAmount, newOrder.CreatedDate);

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
            orderItemResponse.DentalName = dental.Name;
            orderItemResponse.DentistName = order.DentistName;
            orderItemResponse.DentistNote = order.DentistNote;
            orderItemResponse.PatientName = order.PatientName;
            orderItemResponse.PatientGender = order.PatientGender;
            orderItemResponse.Status = EnumUtil.ParseEnum<OrderStatus>(order.Status);
            orderItemResponse.Mode = EnumUtil.ParseEnum<OrderMode>(order.Mode);
            orderItemResponse.TotalAmount = order.TotalAmount;
            orderItemResponse.Discount = order.Discount;
            orderItemResponse.FinalAmount = order.FinalAmount;
            orderItemResponse.CreatedDate = order.CreatedDate;


            orderItemResponse.ToothList = (List<OrderItemResponse>)await _unitOfWork.GetRepository<OrderItem>()
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
                        TotalAmount= x.TotalAmount
                    },
                    predicate: x => x.OrderId.Equals(id)

                );

            return orderItemResponse;

        }
    }
}
