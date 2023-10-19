using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalLabManagement.API.Services;
using System.Linq.Expressions;
using DentalLabManagement.API.Extensions;

namespace DentalLabManagement.API.Services.Implements
{
    public class TeethPositionService : BaseService<TeethPositionService>, ITeethPositionServices
    {
        public TeethPositionService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<TeethPositionService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<TeethPositionResponse> CreateTeethPosition(TeethPositionRequest teethPositionRequest)
        {
            TeethPosition teethPosition = await _unitOfWork.GetRepository<TeethPosition>().SingleOrDefaultAsync
                (predicate: x => x.PositionName.Equals(teethPositionRequest.PositionName));
            if (teethPosition != null) throw new BadHttpRequestException(MessageConstant.TeethPosition.TeethPositionExisted);
            teethPosition = new TeethPosition()
            {
                ToothArch = Convert.ToInt32(teethPositionRequest.ToothArch),
                PositionName = teethPositionRequest.PositionName,
                Description = teethPositionRequest.Description,
            };

            await _unitOfWork.GetRepository<TeethPosition>().InsertAsync(teethPosition);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.TeethPosition.CreateTeethPositionFailed);
            return new TeethPositionResponse(teethPosition.Id, 
                EnumUtil.ParseEnum<ToothArch>(teethPosition.ToothArch.ToString()), 
                teethPosition.PositionName, teethPosition.Description);

        }

        private Expression<Func<TeethPosition, bool>> BuildGetPositionsQuery(string? positionName, ToothArch? toothArch)
        {
            Expression<Func<TeethPosition, bool>> filterQuery = x => true; 

            if (!string.IsNullOrEmpty(positionName))
            {
                filterQuery = filterQuery.AndAlso(x => x.PositionName.Contains(positionName));
            }

            if (toothArch != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.ToothArch.Equals(Convert.ToInt32(toothArch)));
            }

            return filterQuery;
        }


        public async Task<IPaginate<TeethPositionResponse>> GetTeethPositions(string? positionName, ToothArch? toothArch, int page, int size)
        {
            IPaginate<TeethPositionResponse> response = await _unitOfWork.GetRepository<TeethPosition>().GetPagingListAsync(
                selector: x => new TeethPositionResponse(x.Id, EnumUtil.ParseEnum<ToothArch>(x.ToothArch.ToString()),
                    x.PositionName, x.Description),
                predicate: BuildGetPositionsQuery(positionName, toothArch),
                page: page,
                size: size
                ) ;
            return response;
        }

        public async Task<TeethPositionResponse> GetTeethPositionById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.TeethPosition.EmptyTeethPositionIdMessage);
            TeethPosition teethPosition = await _unitOfWork.GetRepository<TeethPosition>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (teethPosition == null) throw new BadHttpRequestException(MessageConstant.TeethPosition.IdNotFoundMessage);
            return new TeethPositionResponse(teethPosition.Id, EnumUtil.ParseEnum<ToothArch>(teethPosition.ToothArch.ToString()),
                teethPosition.PositionName, teethPosition.Description);
        }

        public async Task<TeethPositionResponse> UpdateTeethPosition(int id, UpdateTeethPositionRequest updateTeethPositionRequest)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.TeethPosition.EmptyTeethPositionIdMessage);
            TeethPosition updateTeethPosition = await _unitOfWork.GetRepository<TeethPosition>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (updateTeethPosition == null) throw new BadHttpRequestException(MessageConstant.TeethPosition.IdNotFoundMessage);
            updateTeethPositionRequest.TrimString();

            updateTeethPosition.ToothArch = Convert.ToInt32(updateTeethPositionRequest.ToothArch);
            updateTeethPosition.PositionName = string.IsNullOrEmpty(updateTeethPositionRequest.PositionName)
                ? updateTeethPosition.PositionName : updateTeethPositionRequest.PositionName;
            updateTeethPosition.Description = string.IsNullOrEmpty(updateTeethPositionRequest.Description)
                ? updateTeethPosition.Description : updateTeethPositionRequest.Description;

            _unitOfWork.GetRepository<TeethPosition>().UpdateAsync(updateTeethPosition);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.TeethPosition.UpdateTeethPositionFailedMessage);

            return new TeethPositionResponse(
                updateTeethPosition.Id, EnumUtil.ParseEnum<ToothArch>(updateTeethPosition.ToothArch.ToString()), 
                updateTeethPosition.PositionName, updateTeethPosition.Description);
        }
    }
}
