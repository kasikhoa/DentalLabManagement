﻿using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.ProductStage;
using DentalLabManagement.BusinessTier.Services.Interfaces;
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
            return new ProductStageResponse(productStage.Id, productStage.IndexStage, productStage.Name, productStage.Description, productStage.ExecutionTime);
        }

        public async Task<ProductStageResponse> GetProductStageByIndexStage(int index)
        {
            if (index < 1) throw new HttpRequestException(MessageConstant.ProductStage.EmptyProductStageMessage);
            ProductStage productStage = await _unitOfWork.GetRepository<ProductStage>()
                .SingleOrDefaultAsync(predicate: x => x.IndexStage.Equals(index));
            if (productStage == null) throw new HttpRequestException(MessageConstant.ProductStage.IndexStageNotFoundMessage);
            return new ProductStageResponse(productStage.Id, productStage.IndexStage, productStage.Name, productStage.Description, productStage.ExecutionTime);
        }

        public async Task<ProductStageResponse> GetProductStageById(int id)
        {
            if (id < 1) throw new HttpRequestException(MessageConstant.ProductStage.EmptyProductStageIdMessage);
            ProductStage productStage = await _unitOfWork.GetRepository<ProductStage>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (productStage == null) throw new HttpRequestException(MessageConstant.ProductStage.IdNotFoundMessage);
            return new ProductStageResponse(productStage.Id, productStage.IndexStage, productStage.Name, productStage.Description, productStage.ExecutionTime);
        }


        public async Task<IPaginate<ProductStageResponse>> GetProductStages(string? name, int page, int size)
        {
            name = name?.Trim().ToLower();
            IPaginate<ProductStageResponse> response = await _unitOfWork.GetRepository<ProductStage>().GetPagingListAsync
                (selector: x => new ProductStageResponse(x.Id, x.IndexStage, x.Name, x.Description, x.ExecutionTime),
                predicate: string.IsNullOrEmpty(name) ? x => true : x => x.Name.ToLower().Contains(name),
                page: page,
                size: size
                );
            return response;

        }

        public async Task<ProductStageResponse> UpdateProductStage(int id, UpdateProductStageRequest updateProductStageRequest)
        {
            if (id < 1) throw new HttpRequestException(MessageConstant.ProductStage.EmptyProductStageIdMessage);
            ProductStage updateProductStage = await _unitOfWork.GetRepository<ProductStage>()
               .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (updateProductStage == null) throw new HttpRequestException(MessageConstant.ProductStage.IdNotFoundMessage);
            updateProductStageRequest.TrimString();

            updateProductStage.IndexStage = (updateProductStageRequest.IndexStage) < 1 ? updateProductStage.IndexStage : updateProductStageRequest.IndexStage;
            updateProductStage.Name = string.IsNullOrEmpty(updateProductStageRequest.Name) ? updateProductStage.Name : updateProductStageRequest.Name;
            updateProductStage.Description = string.IsNullOrEmpty(updateProductStageRequest.Description) ? updateProductStage.Description : updateProductStageRequest.Description;
            updateProductStage.ExecutionTime = (updateProductStageRequest.ExecutionTime) <= 0 ? updateProductStage.ExecutionTime : updateProductStageRequest.ExecutionTime;

            _unitOfWork.GetRepository<ProductStage>().UpdateAsync(updateProductStage);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new HttpRequestException(MessageConstant.ProductStage.UpdateProductStageFailedMessage);
            return new ProductStageResponse
                (updateProductStage.Id, updateProductStage.IndexStage, updateProductStage.Name, updateProductStage.Description, updateProductStage.ExecutionTime);
        }

        public async Task<IPaginate<ProductStageResponse>> GetProductStageByCategory(int categoryId, int page, int size)
        {
            List<int> categoryIds = (List<int>)await _unitOfWork.GetRepository<GroupStage>().GetListAsync(
             selector: x => x.ProductStageId,
             predicate: x => x.CategoryId.Equals(categoryId)
             );

            IPaginate<ProductStageResponse> productStageResponse =
            await _unitOfWork.GetRepository<ProductStage>().GetPagingListAsync(
                selector: x => new ProductStageResponse(x.Id, x.IndexStage, x.Name, x.Description, x.ExecutionTime),
                predicate: x => categoryIds.Contains(x.Id),
                orderBy: x => x.OrderBy(x => x.IndexStage),
                page: page,
                size: size
                );
            return productStageResponse;
        }

       
    }
}
