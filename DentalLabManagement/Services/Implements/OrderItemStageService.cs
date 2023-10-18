using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Category;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using DentalLabManagement.BusinessTier.Services;
using DentalLabManagement.BusinessTier.Services.Implements;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Repository.Interfaces;

namespace DentalLabManagement.API.Services.Implements
{
    public class OrderItemStageService : BaseService<OrderItemStageService>, IOrderItemStageService
    {
        public OrderItemStageService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderItemStageService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<UpdateOrderItemStageResponse> UpdateOrderItemStage(int orderItemStageId, UpdateOrderItemStageRequest updateOrderItemStageRequest)
        {

            OrderItemStage orderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderItemStageId));
            if (orderItemStage == null) throw new BadHttpRequestException(MessageConstant.OrderItemStage.EmptyOrderItemStageIdMessage);

            List<int> listIndexStages = (List<int>) await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync( 
                selector: x => x.IndexStage,
                predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId),
                orderBy: x => x.OrderBy(x => x.IndexStage));


            OrderItemStageStatus status = updateOrderItemStageRequest.Status;

            switch (status)
            {
                case OrderItemStageStatus.Pending:

                        if (orderItemStage.IndexStage == listIndexStages[0])
                        {
                            orderItemStage.StaffId = updateOrderItemStageRequest.StaffId;
                            orderItemStage.Status = updateOrderItemStageRequest.Status.GetDescriptionFromEnum();
                            orderItemStage.Note = updateOrderItemStageRequest.Note;
                        }
                        else
                        {
                            OrderItemStage updateItemStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                                predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.IndexStage.Equals(orderItemStage.IndexStage - 1));
                            if (updateItemStage != null && updateItemStage.Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()))
                            {
                                orderItemStage.StaffId = updateOrderItemStageRequest.StaffId;
                                orderItemStage.Status = updateOrderItemStageRequest.Status.GetDescriptionFromEnum();
                                orderItemStage.Note = updateOrderItemStageRequest.Note;
                            }
                            else
                            {
                                return new UpdateOrderItemStageResponse(orderItemStage.Id, await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                                    selector: x => x.FullName, predicate: x => x.Id.Equals(orderItemStage.StaffId)), orderItemStage.IndexStage,
                                    orderItemStage.StageName, orderItemStage.ExecutionTime, EnumUtil.ParseEnum<OrderItemStageStatus>(orderItemStage.Status),
                                    orderItemStage.StartDate, orderItemStage.EndDate, orderItemStage.Note, orderItemStage.Image,
                                    MessageConstant.OrderItemStage.UpdateStatusStageFailedMessage);
                            }
                            
                        }
                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.OrderItemStage.UpdateStatusStageFailedMessage);
                    return new UpdateOrderItemStageResponse(orderItemStage.Id, await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        selector: x => x.FullName, predicate: x => x.Id.Equals(orderItemStage.StaffId)), orderItemStage.IndexStage,
                        orderItemStage.StageName, orderItemStage.ExecutionTime, EnumUtil.ParseEnum<OrderItemStageStatus>(orderItemStage.Status),
                        orderItemStage.StartDate, orderItemStage.EndDate, orderItemStage.Note, orderItemStage.Image, 
                        MessageConstant.OrderItemStage.UpdateStatusStageSuccessMessage);

                 case OrderItemStageStatus.Completed:

                    orderItemStage.Status = updateOrderItemStageRequest.Status.GetDescriptionFromEnum();
                    orderItemStage.Note = updateOrderItemStageRequest.Note;
                    orderItemStage.EndDate = TimeUtils.GetCurrentSEATime();

                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.OrderItemStage.UpdateStatusStageFailedMessage);
                    return new UpdateOrderItemStageResponse(orderItemStage.Id, await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        selector: x => x.FullName, predicate: x => x.Id.Equals(orderItemStage.StaffId)), orderItemStage.IndexStage,
                        orderItemStage.StageName, orderItemStage.ExecutionTime, EnumUtil.ParseEnum<OrderItemStageStatus>(orderItemStage.Status),
                        orderItemStage.StartDate, TimeUtils.GetCurrentSEATime(), orderItemStage.Note, orderItemStage.Image, 
                        MessageConstant.OrderItemStage.UpdateStatusStageSuccessMessage);

                case OrderItemStageStatus.Canceled:

                    orderItemStage.StaffId = updateOrderItemStageRequest.StaffId;
                    orderItemStage.Status = updateOrderItemStageRequest.Status.GetDescriptionFromEnum();
                    orderItemStage.Note = updateOrderItemStageRequest.Note;

                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    isSuccessful = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.OrderItemStage.UpdateStatusStageFailedMessage);
                    return new UpdateOrderItemStageResponse(orderItemStage.Id, await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        selector: x => x.FullName, predicate: x => x.Id.Equals(orderItemStage.StaffId)), orderItemStage.IndexStage,
                        orderItemStage.StageName, orderItemStage.ExecutionTime, EnumUtil.ParseEnum<OrderItemStageStatus>(orderItemStage.Status),
                        orderItemStage.StartDate, orderItemStage.EndDate, orderItemStage.Note, orderItemStage.Image, 
                        MessageConstant.OrderItemStage.UpdateStatusStageSuccessMessage);

                default:

                    return new UpdateOrderItemStageResponse(orderItemStage.Id, await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        selector: x => x.FullName, predicate: x => x.Id.Equals(orderItemStage.StaffId)), orderItemStage.IndexStage,
                        orderItemStage.StageName, orderItemStage.ExecutionTime, EnumUtil.ParseEnum<OrderItemStageStatus>(orderItemStage.Status),
                        orderItemStage.StartDate, orderItemStage.EndDate, orderItemStage.Note, orderItemStage.Image, 
                        MessageConstant.OrderItemStage.UpdateStatusStageSuccessMessage);
            }

        }

    }
}
