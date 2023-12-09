using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Partner;
using DentalLabManagement.API.Services.Interfaces;
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
using System.Linq.Expressions;
using DentalLabManagement.API.Extensions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DentalLabManagement.API.Services.Implements
{
    public class PartnerService : BaseService<IPartnerService>, IPartnerService
    {
        public PartnerService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<IPartnerService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<int> CreateNewPartner(PartnerRequest request)
        {
            Partner newPartner = await _unitOfWork.GetRepository<Partner>().SingleOrDefaultAsync(
                predicate: x => x.Name.Equals(request.Name)
                );
            if (newPartner != null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNameExisted);

            newPartner = new Partner()
            {
                Name = request.Name,
                Address = request.Address,
                Status = PartnerStatus.Active.GetDescriptionFromEnum(),
                Type = request.Type.GetDescriptionFromEnum()
            };

            await _unitOfWork.GetRepository<Partner>().InsertAsync(newPartner);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Dental.CreateDentalFailed);
            return newPartner.Id;
        }

        public async Task<PartnerResponse> GetPartnerById(int dentalId)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Partner partner = await _unitOfWork.GetRepository<Partner>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId)
                );
            if (partner == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            return new PartnerResponse(partner.Id, partner.Name, partner.Address,
                EnumUtil.ParseEnum<PartnerStatus>(partner.Status), EnumUtil.ParseEnum<PartnerType>(partner.Type), partner.AccountId);
        }

        private static Expression<Func<Partner, bool>> BuildGetPartnersQuery(PartnerFilter filter)
        {
            Expression<Func<Partner, bool>> filterQuery = x => true;

            var searchName = filter.name;
            var searchAddress = filter.address;
            var status = filter.status;
            var type = filter.type;

            if (!string.IsNullOrEmpty(searchName))
            {
                filterQuery = filterQuery.AndAlso(x => x.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchAddress))
            {
                filterQuery = filterQuery.AndAlso(x => x.Address.Contains(searchAddress));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            if (type != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Type.Equals(type.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<PartnerResponse>> GetAllPartners(PartnerFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<PartnerResponse> response = await _unitOfWork.GetRepository<Partner>().GetPagingListAsync(
                selector: x => new PartnerResponse(x.Id, x.Name, x.Address, EnumUtil.ParseEnum<PartnerStatus>(x.Status), 
                    EnumUtil.ParseEnum<PartnerType>(x.Type), x.AccountId),
                predicate: BuildGetPartnersQuery(filter),
                page: page,
                size: size
                );
            return response;
        }

        public async Task<bool> UpdatePartnerInfo(int dentalId, UpdatePartnerRequest request)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Partner updateDental = await _unitOfWork.GetRepository<Partner>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId)
                );
            if (updateDental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            Partner otherDental = await _unitOfWork.GetRepository<Partner>().SingleOrDefaultAsync(
                predicate: x => x.AccountId.Equals(request.AccountId) && x.Id != dentalId
                );
            if (otherDental != null) throw new BadHttpRequestException(MessageConstant.Account.AccountExisted);

            request.TrimString();

            Account dentalAccount = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.AccountId)
                );
            if (dentalAccount == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            else if (dentalAccount.Role.Equals(RoleEnum.Partner.GetDescriptionFromEnum()))
            {
                updateDental.Name = string.IsNullOrEmpty(request.Name) ? updateDental.Name : request.Name;
                updateDental.Address = string.IsNullOrEmpty(request.Address) ? updateDental.Address : request.Address;
                updateDental.Status = request.Status.GetDescriptionFromEnum();
                updateDental.AccountId = request.AccountId < 1 || string.IsNullOrEmpty(request.AccountId.ToString()) ? null : request.AccountId;
            }
            else throw new BadHttpRequestException(MessageConstant.Account.CreateAccountWithWrongRoleMessage);

            if (updateDental.Status.Equals(PartnerStatus.Active.GetDescriptionFromEnum()))
            {
                dentalAccount.Status = AccountStatus.Activate.GetDescriptionFromEnum();
            }
            else
            {
                dentalAccount.Status = AccountStatus.Deactivate.GetDescriptionFromEnum();
            }

            _unitOfWork.GetRepository<Partner>().UpdateAsync(updateDental);
            _unitOfWork.GetRepository<Account>().UpdateAsync(dentalAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> DeletePartner(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Partner dental = await _unitOfWork.GetRepository<Partner>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
                );
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            dental.Status = PartnerStatus.Inactive.GetDescriptionFromEnum();

            Account accountDental = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dental.AccountId)
                );

            if (accountDental != null && dental.Status.Equals(PartnerStatus.Inactive.GetDescriptionFromEnum()))
            {
                accountDental.Status = AccountStatus.Deactivate.GetDescriptionFromEnum();
                _unitOfWork.GetRepository<Account>().UpdateAsync(accountDental);
            }
            _unitOfWork.GetRepository<Partner>().UpdateAsync(dental);           
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

    }
}
