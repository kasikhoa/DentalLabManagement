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
using System.Text.RegularExpressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class TeethPositionService : BaseService<TeethPositionService>, ITeethPositionServices
    {
        public TeethPositionService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<TeethPositionService> logger) : base(unitOfWork, logger)
        {

        }

        private bool IsValidPositionName(string positionName)
        {
            string pattern = @"^\d-\d$"; 
            return Regex.IsMatch(positionName, pattern);
        }

        public async Task<TeethPositionResponse> CreateTeethPosition(TeethPositionRequest teethPositionRequest)
        {
            TeethPosition teethPosition = await _unitOfWork.GetRepository<TeethPosition>().SingleOrDefaultAsync
                (predicate: x => x.PositionName.Equals(teethPositionRequest.PositionName));
            if (teethPosition != null) throw new BadHttpRequestException(MessageConstant.TeethPosition.TeethPositionExisted);

            if (!IsValidPositionName(teethPositionRequest.PositionName))
                throw new BadHttpRequestException(MessageConstant.TeethPosition.NameFormatMessage);
            teethPosition = new TeethPosition()
            {
                ToothArch = Convert.ToInt32(teethPositionRequest.ToothArch),
                PositionName = teethPositionRequest.PositionName,
                Description = teethPositionRequest.Description,
                Image = teethPositionRequest.Image
            };

            await _unitOfWork.GetRepository<TeethPosition>().InsertAsync(teethPosition);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.TeethPosition.CreateTeethPositionFailed);
            return new TeethPositionResponse(teethPosition.Id, 
                EnumUtil.ParseEnum<ToothArch>(teethPosition.ToothArch.ToString()), 
                teethPosition.PositionName, teethPosition.Description, teethPosition.Image);

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
            positionName = positionName?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<TeethPositionResponse> response = await _unitOfWork.GetRepository<TeethPosition>().GetPagingListAsync(
                selector: x => new TeethPositionResponse(x.Id, EnumUtil.ParseEnum<ToothArch>(x.ToothArch.ToString()),
                    x.PositionName, x.Description, x.Image),
                predicate: BuildGetPositionsQuery(positionName, toothArch),
                page: page,
                size: size
                );
            return response;
        }

        public async Task<TeethPositionResponse> GetTeethPositionById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.TeethPosition.EmptyTeethPositionIdMessage);
            TeethPosition teethPosition = await _unitOfWork.GetRepository<TeethPosition>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (teethPosition == null) throw new BadHttpRequestException(MessageConstant.TeethPosition.IdNotFoundMessage);
            return new TeethPositionResponse(teethPosition.Id, EnumUtil.ParseEnum<ToothArch>(teethPosition.ToothArch.ToString()),
                teethPosition.PositionName, teethPosition.Description, teethPosition.Image);
        }

        public async Task<bool> UpdateTeethPosition(int id, UpdateTeethPositionRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.TeethPosition.EmptyTeethPositionIdMessage);
            TeethPosition teethPosition = await _unitOfWork.GetRepository<TeethPosition>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (teethPosition == null) throw new BadHttpRequestException(MessageConstant.TeethPosition.IdNotFoundMessage);
            request.TrimString();

            if (!IsValidPositionName(request.PositionName))
                throw new BadHttpRequestException(MessageConstant.TeethPosition.NameFormatMessage);

            teethPosition.ToothArch = Convert.ToInt32(request.ToothArch);
            teethPosition.PositionName = request.PositionName;
            teethPosition.Description = string.IsNullOrEmpty(request.Description) ? teethPosition.Description : request.Description;
            teethPosition.Image = string.IsNullOrEmpty(request.Image) ? teethPosition.Image : request.Image;

            _unitOfWork.GetRepository<TeethPosition>().UpdateAsync(teethPosition);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
