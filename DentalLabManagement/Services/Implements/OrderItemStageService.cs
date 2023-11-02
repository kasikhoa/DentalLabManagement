using AutoMapper;
using DentalLabManagement.API.Extensions;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.OrderItemStage;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class OrderItemStageService : BaseService<OrderItemStageService>, IOrderItemStageService
    {
        public OrderItemStageService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderItemStageService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        private Expression<Func<OrderItemStage, bool>> BuildGetOrderItemStagesQuery(int? orderId, int? orderItemId, int? staffId, int? indexStage, 
            OrderItemStageStatus? status, OrderItemStageMode? mode)
        {
            Expression<Func<OrderItemStage, bool>> filterQuery = x => true;

            if (orderId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.OrderItem.OrderId.Equals(orderId));
            }

            if (orderItemId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.OrderItemId.Equals(orderItemId));
            }
            if (staffId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.StaffId.Equals(staffId));
            }

            if (indexStage.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.IndexStage.Equals(indexStage));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            if (mode != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Mode.Equals(mode.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<OrderItemStageResponse>> GetOrderItemStages(int? orderId, int? orderItemId, int? staffId, int? indexStage, OrderItemStageStatus? status, 
            OrderItemStageMode? mode, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<OrderItemStageResponse> result = await _unitOfWork.GetRepository<OrderItemStage>().GetPagingListAsync(
                selector: x => new OrderItemStageResponse()
                {
                    Id = x.Id,
                    OrderItemId = x.OrderItemId,
                    StaffName = x.Staff.FullName,
                    IndexStage = x.IndexStage,
                    StageName = x.StageName,
                    Description = x.Description,
                    Execution = x.ExecutionTime,
                    Status = EnumUtil.ParseEnum<OrderItemStageStatus>(x.Status),
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Note = x.Note,
                    Image = x.Image
                },
                predicate: BuildGetOrderItemStagesQuery(orderId, orderItemId, staffId, indexStage, status, mode),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<OrderItemStageResponse> GetOrderItemStageById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.OrderItemStage.EmptyOrderItemStageIdMessage);
            OrderItemStage orderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.Staff)
                );
            if (orderItemStage == null) throw new BadHttpRequestException(MessageConstant.OrderItemStage.OrderItemStageNotFoundMessage);

            string staffName = (orderItemStage.Staff == null) ? null : orderItemStage.Staff.FullName;
            return new OrderItemStageResponse()
            {
                Id = orderItemStage.Id,
                OrderItemId = orderItemStage.Id,
                StaffName = staffName,
                IndexStage = orderItemStage.IndexStage,
                StageName = orderItemStage.StageName,
                Description = orderItemStage.Description,
                Execution = orderItemStage.ExecutionTime,
                Status = EnumUtil.ParseEnum<OrderItemStageStatus>(orderItemStage.Status),
                StartDate = orderItemStage.StartDate,
                EndDate = orderItemStage.EndDate,
                Note = orderItemStage.Note,
                Image = orderItemStage.Image,
            };
        }

        public async Task<bool> UpdateOrderItemStage(int orderItemStageId, UpdateOrderItemStageRequest request)
        {
            if (orderItemStageId < 1) throw new BadHttpRequestException(MessageConstant.OrderItemStage.EmptyOrderItemStageIdMessage);
            OrderItemStage orderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(orderItemStageId)
                );
            if (orderItemStage == null) throw new BadHttpRequestException(MessageConstant.OrderItemStage.OrderItemStageNotFoundMessage);

            string currentUser = GetUsernameFromJwt();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.UserName.Equals(currentUser));

            List<int> listIndexStages = (List<int>) await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
                selector: x => x.IndexStage,
                predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.Mode.Equals(orderItemStage.Mode),
                orderBy: x => x.OrderBy(x => x.IndexStage)
                );

            OrderItemStageStatus status = request.Status;

            switch (status)
            {
                case OrderItemStageStatus.Pending:

                    if (orderItemStage.IndexStage == listIndexStages[0])
                    {
                        orderItemStage.StaffId = account.Id;
                        orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                        orderItemStage.Note = request.Note;
                    }
                    else
                    {
                        OrderItemStage prevOrderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                            predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.IndexStage.Equals(orderItemStage.IndexStage - 1)
                            && x.Mode.Equals(orderItemStage.Mode));
                        if (prevOrderItemStage != null && prevOrderItemStage.Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()))
                        {
                            orderItemStage.StaffId = account.Id;
                            orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                            orderItemStage.Note = request.Note;
                        }
                        else
                        {
                            throw new BadHttpRequestException(MessageConstant.OrderItemStage.PreviousStageNotCompletedMessage);
                        }

                    }
                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                case OrderItemStageStatus.Completed:

                    if (orderItemStage.IndexStage == listIndexStages[0])
                    {
                        orderItemStage.StaffId = account.Id;
                        orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                        orderItemStage.Note = request.Note;
                        orderItemStage.EndDate = TimeUtils.GetCurrentSEATime();
                    }
                    else
                    {
                        OrderItemStage prevOrderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                            predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.IndexStage.Equals(orderItemStage.IndexStage - 1)
                            && x.Mode.Equals(orderItemStage.Mode));
                        if (prevOrderItemStage != null && prevOrderItemStage.Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()))
                        {
                            orderItemStage.StaffId = account.Id;
                            orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                            orderItemStage.Note = request.Note;
                            orderItemStage.EndDate = TimeUtils.GetCurrentSEATime();
                        }
                        else
                        {
                            throw new BadHttpRequestException(MessageConstant.OrderItemStage.PreviousStageNotCompletedMessage);
                        }

                    }
                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                case OrderItemStageStatus.Canceled:

                    orderItemStage.StaffId = account.Id;
                    orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                    orderItemStage.Note = request.Note;

                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                default:

                    throw new BadHttpRequestException(MessageConstant.OrderItemStage.UpdateStatusStageFailedMessage);
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;

        }

    }
}
