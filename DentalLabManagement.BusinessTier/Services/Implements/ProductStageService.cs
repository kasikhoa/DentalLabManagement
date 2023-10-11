using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Implements
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
            if (productStage != null) throw new HttpRequestException(MessageConstant.ProductStage.ProductStageExisted);
            productStage = new ProductStage()
            {
                IndexStage = productStageRequest.IndexStage,
                Name = productStageRequest.Name,
                Description = productStageRequest.Description,
            };
            await _unitOfWork.GetRepository<ProductStage>().InsertAsync(productStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new HttpRequestException(MessageConstant.ProductStage.CreateNewProductStageFailed);
            return new ProductStageResponse(productStage.Id, productStage.IndexStage, productStage.Name, productStage.Description);
        }

        public async Task<ProductStageResponse> GetProductStageByIndexStage(int index)
        {
            if (index < 1) throw new HttpRequestException(MessageConstant.ProductStage.EmptyProductStageMessage);
            ProductStage productStage = await _unitOfWork.GetRepository<ProductStage>()
                .SingleOrDefaultAsync(predicate: x => x.IndexStage.Equals(index));
            if (productStage == null) throw new HttpRequestException(MessageConstant.ProductStage.IndexStageNotFoundMessage);
            return new ProductStageResponse(productStage.Id, productStage.IndexStage, productStage.Name, productStage.Description);
        }

        public async Task<IPaginate<ProductStageResponse>> GetProductStages(string? name, int page, int size)
        {
            name = name?.Trim().ToLower();
            IPaginate<ProductStageResponse> response = await _unitOfWork.GetRepository<ProductStage>().GetPagingListAsync
                (selector: x => new ProductStageResponse(x.Id, x.IndexStage, x.Name, x.Description),
                predicate: string.IsNullOrEmpty(name) ? x => true : x => x.Name.ToLower().Contains(name),
                page: page,
                size: size
                );
            return response;

        }
    }
}
