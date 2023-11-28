using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DentalLabManagement.API.Extensions;
using AutoMapper;
using DentalLabManagement.BusinessTier.Payload.ProductionStage;
using Microsoft.EntityFrameworkCore;

namespace DentalLabManagement.API.Services.Implements
{
    public class ProductionStageService : BaseService<ProductionStageService>, IProductionStageService
    {
        public ProductionStageService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<ProductionStageService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<ProductionStageResponse> CreateProductionStage(ProductionStageRequest request)
        {
            ProductionStage productionStage = await _unitOfWork.GetRepository<ProductionStage>().SingleOrDefaultAsync
                (predicate: x => x.Name.Equals(request.Name),
                include: x => x.Include(x => x.ProductStageMappings)
                );
            if (productionStage != null) throw new BadHttpRequestException(MessageConstant.ProductionStage.ProductStageExisted);

            productionStage = new ProductionStage()
            {
                Name = request.Name,
                Description = request.Description,
                ExecutionTime = request.ExecutionTime,
            };

            await _unitOfWork.GetRepository<ProductionStage>().InsertAsync(productionStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.ProductionStage.CreateNewProductStageFailed);

            return new ProductionStageResponse(productionStage.Id, productionStage.Name, productionStage.Description, productionStage.ExecutionTime);
        }

        public async Task<ProductionStageResponse> GetProductionStageById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.ProductionStage.EmptyProductStageIdMessage);

            ProductionStage productionStage = await _unitOfWork.GetRepository<ProductionStage>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.ProductStageMappings)
                );
            if (productionStage == null) throw new BadHttpRequestException(MessageConstant.ProductionStage.NotFoundMessage);

            return new ProductionStageResponse(productionStage.Id, productionStage.Name, productionStage.Description, productionStage.ExecutionTime);
        }

        private Expression<Func<ProductionStage, bool>> BuildGetStageQuery(ProductionStageFilter filter)
        {
            Expression<Func<ProductionStage, bool>> filterQuery = p => true;

            var name = filter.name;
           
            if (!string.IsNullOrEmpty(name))
            {
                filterQuery = filterQuery.AndAlso(p => p.Name.Contains(name));
            }

            return filterQuery;
        }

        public async Task<IPaginate<ProductionStageResponse>> GetProductionStages(ProductionStageFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<ProductionStageResponse> response = await _unitOfWork.GetRepository<ProductionStage>().GetPagingListAsync
                (selector: x => new ProductionStageResponse(x.Id, x.Name, x.Description, x.ExecutionTime),
                predicate: BuildGetStageQuery(filter),
                orderBy: x => x.OrderBy(x => x.Id),
                page: page,
                size: size
                );
            return response;
        }

        public async Task<bool> UpdateProductionStage(int id, UpdateProductionStageRequest request)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.ProductionStage.EmptyProductStageIdMessage);
            ProductionStage productionStage = await _unitOfWork.GetRepository<ProductionStage>()
               .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (productionStage == null) throw new BadHttpRequestException(MessageConstant.ProductionStage.NotFoundMessage);
            request.TrimString();

            productionStage.Name = string.IsNullOrEmpty(request.Name) ? productionStage.Name : request.Name;
            productionStage.Description = string.IsNullOrEmpty(request.Description) ? productionStage.Description : request.Description;
            productionStage.ExecutionTime = (request.ExecutionTime < 1) ? productionStage.ExecutionTime : request.ExecutionTime;

            _unitOfWork.GetRepository<ProductionStage>().UpdateAsync(productionStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }     
     
    }
}
