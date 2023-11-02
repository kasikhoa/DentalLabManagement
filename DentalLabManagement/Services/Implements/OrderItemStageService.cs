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

        private Expression<Func<OrderItemStage, bool>> BuildGetOrderItemStagesQuery(int? orderId, int? orderItemId, int? accountId, int? staffId, int? stageId,
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

            if (accountId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.StageId.Equals(accountId));
            }

            if (staffId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.StaffId.Equals(staffId));
            }

            if (stageId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.StageId.Equals(stageId));
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

        public async Task<IPaginate<OrderItemStageResponse>> GetOrderItemStages(int? orderId, int? orderItemId, int? accountId, int? staffId, int? stageId, OrderItemStageStatus? status,
            OrderItemStageMode? mode, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            int? currentStage = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(accountId),
                selector: x => x.CurrentStage
                );

            IPaginate<OrderItemStageResponse> result = await _unitOfWork.GetRepository<OrderItemStage>().GetPagingListAsync(
                selector: x => new OrderItemStageResponse()
                {
                    Id = x.Id,
                    OrderItemId = x.OrderItemId,
                    StaffName = x.Staff.FullName,
                    IndexStage = x.Stage.IndexStage,
                    StageName = x.Stage.Name,
                    Description = x.Stage.Description,
                    StartTime = x.StartTime,
                    ExpectedTime = x.ExpectedTime,
                    CompletedTime = x.CompletedTime,
                    Status = EnumUtil.ParseEnum<OrderItemStageStatus>(x.Status),
                    Mode = EnumUtil.ParseEnum<OrderItemStageMode>(x.Mode),
                    Note = x.Note,
                    Image = x.Image
                },
                predicate: BuildGetOrderItemStagesQuery(orderId, orderItemId, currentStage, staffId, stageId, status, mode),
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
                include: x => x.Include(x => x.Staff).Include(x => x.Stage)
                );
            if (orderItemStage == null) throw new BadHttpRequestException(MessageConstant.OrderItemStage.OrderItemStageNotFoundMessage);

            return new OrderItemStageResponse()
            {
                Id = orderItemStage.Id,
                OrderItemId = orderItemStage.OrderItemId,
                StaffName = orderItemStage.Staff?.FullName,
                IndexStage = orderItemStage.Stage.IndexStage,
                StageName = orderItemStage.Stage.Name,
                Description = orderItemStage.Stage.Description,
                StartTime = orderItemStage.StartTime,
                ExpectedTime = orderItemStage.ExpectedTime,
                CompletedTime = orderItemStage.CompletedTime,
                Status = EnumUtil.ParseEnum<OrderItemStageStatus>(orderItemStage.Status),
                Mode = EnumUtil.ParseEnum<OrderItemStageMode>(orderItemStage.Mode),
                Note = orderItemStage.Note,
                Image = orderItemStage.Image
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

            //if (!account.CurrentStage.Equals(orderItemStage.StageId))
            //{
            //    throw new BadHttpRequestException(MessageConstant.Account.StaffNotMatchStageMessage);
            //}

            string Note = string.IsNullOrEmpty(request.Note) ? null : request.Note;

            OrderItemStageStatus status = request.Status;

            switch (status)
            {
                case OrderItemStageStatus.Pending:

                    orderItemStage.StaffId = account.Id;
                    orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                    orderItemStage.Note = Note;

                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                case OrderItemStageStatus.Completed:

                    orderItemStage.StaffId = account.Id;
                    orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                    orderItemStage.Note = Note;
                    orderItemStage.CompletedTime = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                case OrderItemStageStatus.Canceled:

                    orderItemStage.StaffId = account.Id;
                    orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                    orderItemStage.Note = Note;

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
