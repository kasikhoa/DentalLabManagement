using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Implements
{
    public class ProductStageService : BaseService<ProductStageService>, IProductStageService
    {

        public ProductStageService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<ProductStageService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<ProductStageResponse> CreateProductStage(ProductStageRequest productStageRequest)
        {
            ProductStage productStage = await _unitOfWork.GetRepository<ProductStage>().SingleOrDefaultAsync
                (predicate: x => x.Name.Equals(productStageRequest.Name));
            if (productStage != null) throw new BadHttpRequestException(MessageConstant.ProductStage.ProductStageExisted);

            productStage = new ProductStage()
            {
                IndexStage = productStageRequest.IndexStage,
                Name = productStageRequest.Name,
                Description = productStageRequest.Description,
            };

            await _unitOfWork.GetRepository<ProductStage>().InsertAsync(productStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.ProductStage.CreateNewProductStageFailed);

            return new ProductStageResponse(productStage.Id, productStage.IndexStage, productStage.Name, productStage.Description, productStage.ExecutionTime);
        }

        public async Task<ProductStageResponse> GetProductStageById(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.ProductStage.EmptyProductStageIdMessage);

            ProductStage productStage = await _unitOfWork.GetRepository<ProductStage>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (productStage == null) throw new BadHttpRequestException(MessageConstant.ProductStage.IdNotFoundMessage);

            return new ProductStageResponse(productStage.Id, productStage.IndexStage, 
                productStage.Name, productStage.Description, productStage.ExecutionTime);
        }


        public async Task<IPaginate<ProductStageResponse>> GetProductStages(string? name, int? index, int page, int size)
        {
            name = name?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<ProductStageResponse> response = await _unitOfWork.GetRepository<ProductStage>().GetPagingListAsync
                (selector: x => new ProductStageResponse(x.Id, x.IndexStage, x.Name, x.Description, x.ExecutionTime),
                predicate: string.IsNullOrEmpty(name) && !index.HasValue
                    ? x => true
                    : ((!index.HasValue)
                        ? x => x.Name.Contains(name)
                        : (string.IsNullOrEmpty(name)
                            ? x => x.IndexStage.Equals(index)
                            : x => x.Name.Contains(name) && x.IndexStage.Equals(index))),
                orderBy: x => x.OrderBy(x => x.IndexStage),
                page: page,
                size: size
                );
            return response;
        }

        public async Task<bool> UpdateProductStage(int id, UpdateProductStageRequest updateProductStageRequest)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.ProductStage.EmptyProductStageIdMessage);
            ProductStage updateProductStage = await _unitOfWork.GetRepository<ProductStage>()
               .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (updateProductStage == null) throw new BadHttpRequestException(MessageConstant.ProductStage.IdNotFoundMessage);
            updateProductStageRequest.TrimString();

            updateProductStage.IndexStage = (updateProductStageRequest.IndexStage < 1) ? updateProductStage.IndexStage : updateProductStageRequest.IndexStage;
            updateProductStage.Name = string.IsNullOrEmpty(updateProductStageRequest.Name) ? updateProductStage.Name : updateProductStageRequest.Name;
            updateProductStage.Description = string.IsNullOrEmpty(updateProductStageRequest.Description) ? updateProductStage.Description : updateProductStageRequest.Description;
            updateProductStage.ExecutionTime = (updateProductStageRequest.ExecutionTime < 1) ? updateProductStage.ExecutionTime : updateProductStageRequest.ExecutionTime;

            _unitOfWork.GetRepository<ProductStage>().UpdateAsync(updateProductStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }     
     
    }
}
