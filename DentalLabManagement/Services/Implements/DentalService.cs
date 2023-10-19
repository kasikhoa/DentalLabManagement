using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Dental;
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

namespace DentalLabManagement.API.Services.Implements
{
    public class DentalService : BaseService<IDentalService>, IDentalService
    {
        public DentalService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<DentalService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<DentalResponse> CreateDentalAccount(DentalRequest dentalRequest)
        {
            if (dentalRequest.AccountId < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Dental newDental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync
                (predicate: x => x.AccountId.Equals(dentalRequest.AccountId)); 
            if (newDental != null) throw new BadHttpRequestException(MessageConstant.Account.AccountExisted);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(dentalRequest.AccountId));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);
            if (account.Role.Equals(RoleEnum.Dental))
            {
                newDental = new Dental()
                {
                    Name = dentalRequest.DentalName,
                    Address = dentalRequest.Address,
                    AccountId = dentalRequest.AccountId,
                };
            } 
            else throw new BadHttpRequestException(MessageConstant.Account.CreateAccountWithWrongRoleMessage);

            await _unitOfWork.GetRepository<Dental>().InsertAsync(newDental);
            bool isSuccefful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccefful) throw new BadHttpRequestException(MessageConstant.Dental.CreateDentalFailed);
            return new DentalResponse(newDental.Id, newDental.Name, newDental.Address, newDental.AccountId);

        }

        public async Task<DentalAccountResponse> GetAccountDentalById(int dentalId)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);
            
            Dental dental = await _unitOfWork.GetRepository<Dental>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(dentalId));
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            Account dentalAccount = await _unitOfWork.GetRepository<Account>().
                SingleOrDefaultAsync(predicate: x => x.Id.Equals(dental.AccountId));
            if (dentalAccount == null) throw new BadHttpRequestException(MessageConstant.Dental.AccountDentalNotFoundMessage);

            return new DentalAccountResponse(dental.Id, dental.Name, dental.Address, dentalAccount.UserName, 
                EnumUtil.ParseEnum<AccountStatus>(dentalAccount.Status));

        }

        public async Task<IPaginate<DentalResponse>> GetDentalAccounts(string? searchDentalName, int page, int size)
        {
            searchDentalName = searchDentalName?.Trim().ToLower();
            IPaginate<DentalResponse> response = await _unitOfWork.GetRepository<Dental>().GetPagingListAsync(
                selector: x => new DentalResponse(x.Id, x.Name, x.Address, x.AccountId),
                predicate: string.IsNullOrEmpty(searchDentalName) ? x => true : x => x.Name.ToLower().Contains(searchDentalName),
                page: page,
                size: size
                );
            return response;
        }

        public async Task<DentalResponse> UpdateDentalInfo(int dentalId, UpdateDentalRequest updateDentalRequest)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Dental updateDental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId));
            if (updateDental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            updateDentalRequest.TrimString();

            updateDental.Name = string.IsNullOrEmpty(updateDentalRequest.Name) ? updateDental.Name : updateDentalRequest.Name;
            updateDental.Address = string.IsNullOrEmpty(updateDentalRequest.Address) ? updateDental.Address : updateDentalRequest.Address;

            _unitOfWork.GetRepository<Dental>().UpdateAsync(updateDental);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Dental.UpdateDentalFailedMessage);
            return new DentalResponse(updateDental.Id, updateDental.Name, updateDental.Address, updateDental.AccountId);

        }
    }
}
