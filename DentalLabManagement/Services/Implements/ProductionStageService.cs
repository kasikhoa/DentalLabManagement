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
                (predicate: x => x.Name.Equals(request.Name));
            if (productionStage != null) throw new BadHttpRequestException(MessageConstant.ProductionStage.ProductStageExisted);

            productionStage = new ProductionStage()
            {
                IndexStage = request.IndexStage,
                Name = request.Name,
                Description = request.Description,
            };

            await _unitOfWork.GetRepository<ProductionStage>().InsertAsync(productionStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.ProductionStage.CreateNewProductStageFailed);

            return new ProductionStageResponse(productionStage.Id, productionStage.IndexStage, productionStage.Name,
                productionStage.Description, productionStage.ExecutionTime);
        }

        public async Task<ProductionStageResponse> GetProductionStageById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.ProductionStage.EmptyProductStageIdMessage);

            ProductionStage productionStage = await _unitOfWork.GetRepository<ProductionStage>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (productionStage == null) throw new BadHttpRequestException(MessageConstant.ProductionStage.NotFoundMessage);

            return new ProductionStageResponse(productionStage.Id, productionStage.IndexStage, 
                productionStage.Name, productionStage.Description, productionStage.ExecutionTime);
        }

        private Expression<Func<ProductionStage, bool>> BuildGetStageQuery(int? index, string? name)
        {
            Expression<Func<ProductionStage, bool>> filterQuery = p => true;
           
            if (index.HasValue)
            {
                filterQuery = filterQuery.AndAlso(p => p.IndexStage.Equals(index));
            }

            if (!string.IsNullOrEmpty(name))
            {
                filterQuery = filterQuery.AndAlso(p => p.Name.Contains(name));
            }

            return filterQuery;
        }

        public async Task<IPaginate<ProductionStageResponse>> GetProductionStages(int? index, string? name, int page, int size)
        {
            name = name?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<ProductionStageResponse> response = await _unitOfWork.GetRepository<ProductionStage>().GetPagingListAsync
                (selector: x => new ProductionStageResponse(x.Id, x.IndexStage, x.Name, x.Description, x.ExecutionTime),
                predicate: BuildGetStageQuery(index, name),
                orderBy: x => x.OrderBy(x => x.IndexStage),
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

            productionStage.IndexStage = (request.IndexStage < 1) ? productionStage.IndexStage : request.IndexStage;
            productionStage.Name = string.IsNullOrEmpty(request.Name) ? productionStage.Name : request.Name;
            productionStage.Description = string.IsNullOrEmpty(request.Description) ? productionStage.Description : request.Description;
            productionStage.ExecutionTime = (request.ExecutionTime < 1) ? productionStage.ExecutionTime : request.ExecutionTime;

            _unitOfWork.GetRepository<ProductionStage>().UpdateAsync(productionStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }     
     
    }
}
