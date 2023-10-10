﻿using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.DataTier.Models;
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
    }
}
