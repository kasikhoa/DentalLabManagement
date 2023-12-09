using AutoMapper;
using DentalLabManagement.API.Extensions;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.MaterialStock;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using System.Linq.Expressions;

namespace DentalLabManagement.API.Services.Implements
{
    public class MaterialStockService : BaseService<MaterialStockService>, IMaterialStockService
    {
        public MaterialStockService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<MaterialStockService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<int> CreateNewMaterial(MaterialStockRequest request)
        {
            Category category = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.CategoryId)
                ) ?? throw new BadHttpRequestException(MessageConstant.Category.CategoryNotFoundMessage);

            MaterialStock newMaterialStock = await _unitOfWork.GetRepository<MaterialStock>().SingleOrDefaultAsync(
                predicate: x => x.Code.Equals(request.Code)
                );
            if (newMaterialStock != null) throw new BadHttpRequestException(MessageConstant.MaterialStock.MaterialCodeExistedMessage);

            newMaterialStock = new MaterialStock()
            {
                CategoryId = string.IsNullOrEmpty(request.CategoryId.ToString()) ? null : category.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Quantity = 0,
                SupplierPrice = request.SupplierPrice,
                WeightType = (int)request.WeightType,
                Status = MaterialStockStatus.Active.GetDescriptionFromEnum(),
            };
            await _unitOfWork.GetRepository<MaterialStock>().InsertAsync(newMaterialStock);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.MaterialStock.CreateMaterialFailedMessage);
            return newMaterialStock.Id;
        }

        private static Expression<Func<MaterialStock, bool>> BuildGetMaterialStocksQuery(MaterialStockFilter filter)
        {
            Expression<Func<MaterialStock, bool>> filterQuery = x => true;

            var categoryId = filter.categoryId;
            var code = filter.code;
            var name = filter.name; 
            var weightType = filter.weightType;
            var status = filter.status;

            if (categoryId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CategoryId.Equals(categoryId));
            }

            if (!string.IsNullOrEmpty(code))
            {
                filterQuery = filterQuery.AndAlso(x => x.Code.Contains(code));
            }

            if (!string.IsNullOrEmpty(name))
            {
                filterQuery = filterQuery.AndAlso(x => x.Name.Contains(name));
            }

            if (weightType != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.WeightType.Equals((int)weightType));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<MaterialStockResponse>> ViewMaterialStock(MaterialStockFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<MaterialStockResponse> accounts = await _unitOfWork.GetRepository<MaterialStock>().GetPagingListAsync(
                selector: x => new MaterialStockResponse(x.Id, x.CategoryId, x.Code, x.Name, x.Description, x.Quantity, x.SupplierPrice, 
                     EnumUtil.ParseEnum<MaterialWeightType>(x.WeightType.ToString()), EnumUtil.ParseEnum<MaterialStockStatus>(x.Status)),
                predicate: BuildGetMaterialStocksQuery(filter),
                orderBy: x => x.OrderBy(x => x.Quantity),
                page: page,
                size: size
                );
            return accounts;

        }
    }
}
