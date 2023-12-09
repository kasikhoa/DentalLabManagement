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
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class OrderItemStageService : BaseService<OrderItemStageService>, IOrderItemStageService
    {
        public OrderItemStageService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<OrderItemStageService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        private static Expression<Func<OrderItemStage, bool>> BuildGetOrderItemStagesQuery(OrderItemStageFilter filter)
        {
            Expression<Func<OrderItemStage, bool>> filterQuery = x => true;

            var orderId = filter.orderId;
            var orderItemId = filter.orderItemId;
            var accountId = filter.accountId;
            var staffId = filter.staffId;
            var stageId = filter.stageId;
            var startTime = filter.timeFrom;
            var endTime = filter.timeTo;
            var status = filter.status;
            var mode = filter.mode;
            var productType = filter.productType;

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

            if (startTime != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.CompletedTime >= startTime);
            }

            if (endTime != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.CompletedTime <= endTime);
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            if (mode != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Mode.Equals(mode.GetDescriptionFromEnum()));
            }

            if (productType != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.OrderItem.Product.Type.Equals(productType.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<OrderItemStageResponse>> GetOrderItemStages(OrderItemStageFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            filter.accountId = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(filter.accountId),
                selector: x => x.CurrentStage
                );

            IPaginate<OrderItemStageResponse> result = await _unitOfWork.GetRepository<OrderItemStage>().GetPagingListAsync(
                selector: x => new OrderItemStageResponse()
                {
                    Id = x.Id,
                    OrderItemId = x.OrderItemId,
                    StaffId = x.StaffId,
                    IndexStage = x.IndexStage,
                    StageId = x.StageId,
                    Description = x.Stage.Description,
                    StartTime = x.StartTime,
                    ExpectedTime = x.ExpectedTime,
                    CompletedTime = x.CompletedTime,
                    Status = EnumUtil.ParseEnum<OrderItemStageStatus>(x.Status),
                    Mode = EnumUtil.ParseEnum<OrderItemStageMode>(x.Mode),
                    Note = x.Note,
                    Image = x.Image
                },
                predicate: BuildGetOrderItemStagesQuery(filter),
                orderBy: x => x.OrderBy(x => x.IndexStage),
                page: page,
                size: size
                );
            return result;
        }

        public async Task<OrderItemStageResponse> GetOrderItemStageById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.OrderItemStage.EmptyOrderItemStageIdMessage);

            OrderItemStage orderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
                );
            if (orderItemStage == null) throw new BadHttpRequestException(MessageConstant.OrderItemStage.OrderItemStageNotFoundMessage);

            return new OrderItemStageResponse()
            {
                Id = orderItemStage.Id,
                OrderItemId = orderItemStage.OrderItemId,
                StaffId = orderItemStage.StaffId,
                IndexStage = orderItemStage.IndexStage,
                StageId = orderItemStage.StageId,
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
            DateTime currentTime = TimeUtils.GetCurrentSEATime();

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.UserName.Equals(currentUser)
                );

            //if (!account.CurrentStage.Equals(orderItemStage.StageId))
            //{
            //    throw new BadHttpRequestException(MessageConstant.Account.StaffNotMatchStageMessage);
            //}

            //List<int> listStageIds = (List<int>) await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
            //    predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.Mode.Equals(orderItemStage.Mode),
            //    selector: x => x.StageId
            //    );

            //List<int> indexStages = (List<int>) await _unitOfWork.GetRepository<ProductStageMapping>().GetListAsync(
            //    predicate: x => listStageIds.Contains(x.StageId) && x.ProductId.Equals(orderItemStage.Stage.ProductStageMappings.FirstOrDefault().ProductId),
            //    orderBy: x => x.OrderBy(x => x.IndexStage),
            //    selector: x => x.IndexStage
            //    );

            var indexStages = await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
                predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.Mode.Equals(orderItemStage.Mode),
                selector: x => x.IndexStage,
                orderBy: x => x.OrderBy(x => x.IndexStage)
                );

            OrderItemStageStatus status = request.Status;

            switch (status)
            {
                case OrderItemStageStatus.Pending:

                    if (indexStages != null && indexStages.First() == orderItemStage.IndexStage)
                    {
                        orderItemStage.StaffId = account.Id;
                        orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                        orderItemStage.Note = request.Note;
                    }
                    else
                    {
                        var prevOrderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
                            predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.IndexStage < orderItemStage.IndexStage 
                                && x.Mode.Equals(orderItemStage.Mode),
                            include: x => x.Include(x => x.Stage)
                            );
                        if (prevOrderItemStage != null && prevOrderItemStage.Last().Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()))
                        {
                            orderItemStage.StaffId = account.Id;
                            orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                            orderItemStage.Note = request.Note;
                        }
                        else throw new BadHttpRequestException(MessageConstant.OrderItemStage.PreviousStageNotCompletedMessage);
                    }

                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                case OrderItemStageStatus.Completed:

                    if (indexStages.First() == orderItemStage.IndexStage)
                    {
                        orderItemStage.StaffId = account.Id;
                        orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                        orderItemStage.Note = request.Note;
                        orderItemStage.CompletedTime = currentTime;
                    }
                    else
                    {
                        var prevOrderItemStage = await _unitOfWork.GetRepository<OrderItemStage>().GetListAsync(
                            predicate: x => x.OrderItemId.Equals(orderItemStage.OrderItemId) && x.IndexStage < orderItemStage.IndexStage 
                                && x.Mode.Equals(orderItemStage.Mode)
                            );
                        if (prevOrderItemStage != null && prevOrderItemStage.Last().Status.Equals(OrderItemStageStatus.Completed.GetDescriptionFromEnum()))
                        {
                            orderItemStage.StaffId = account.Id;
                            orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                            orderItemStage.Note = request.Note;
                            orderItemStage.CompletedTime = currentTime;
                        }
                        else throw new BadHttpRequestException(MessageConstant.OrderItemStage.PreviousStageNotCompletedMessage);
                    }

                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                case OrderItemStageStatus.Canceled:

                    orderItemStage.StaffId = account.Id;
                    orderItemStage.Status = request.Status.GetDescriptionFromEnum();
                    orderItemStage.Note = request.Note;

                    _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(orderItemStage);
                    break;

                default: return false;
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;

        }

        public async Task<bool> TransferStageToAnother(int id, TransferStageRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.OrderItemStage.EmptyOrderItemStageIdMessage);

            OrderItemStage updateStage = await _unitOfWork.GetRepository<OrderItemStage>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)) 
                ?? throw new BadHttpRequestException(MessageConstant.OrderItemStage.OrderItemStageNotFoundMessage);

            Account staff = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.StaffId)
                );
            if (!staff.Role.Equals(RoleEnum.Staff.GetDescriptionFromEnum())) 
                throw new BadHttpRequestException(MessageConstant.Account.StaffNotFoundMessage);

            updateStage.StaffId = request.StaffId;
            updateStage.Status = OrderItemStageStatus.Pending.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<OrderItemStage>().UpdateAsync(updateStage);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
