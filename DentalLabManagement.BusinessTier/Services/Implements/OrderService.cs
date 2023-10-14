using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string currentTimeStamp = TimeUtils.GetTimestamp(currentTime);

            Dental dental = await _unitOfWork.GetRepository<Dental>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(createOrderRequest.DentalId));
            if (dental == null) throw new HttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);          

            Order newOrder = new Order()
            {
                DentalId = dental.Id,
                DentistName = createOrderRequest.DentistName,
                PatientName = createOrderRequest.PatientName,
                PatientGender = createOrderRequest.PatientGender,
                DentistNote = createOrderRequest.DentistNote,
                Status= createOrderRequest.Status,
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
            return new CreateOrderResponse(newOrder.Id, newInvoice, dental.Name, newOrder.DentistName , newOrder.DentistNote, newOrder.PatientName, newOrder.Status,
                EnumUtil.ParseEnum<OrderMode>(newOrder.Mode), newOrder.TotalAmount, newOrder.Discount, newOrder.FinalAmount, newOrder.CreatedDate);

        }
    }
}
