using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DentalLabManagement.API.Extensions;
using DentalLabManagement.BusinessTier.Payload.Dental;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DentalLabManagement.API.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<LoginResponse?> Login(LoginRequest loginRequest)
        {
            Expression<Func<Account, bool>> searchFilter = p =>
                p.UserName.Equals(loginRequest.Username) &&
                p.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: searchFilter
                );
            if (account == null) return null;
           
            var token = JwtUtil.GenerateJwtToken(account);
            
            LoginResponse loginResponse = new LoginResponse()
            {
                AccessToken = token,
                Id = account.Id,
                Username = account.UserName,
                Role = EnumUtil.ParseEnum<RoleEnum>(account.Role),
                Status = EnumUtil.ParseEnum<AccountStatus>(account.Status),
            };
            return loginResponse;
        }

        public async Task<GetAccountsResponse> CreateNewAccount(AccountRequest request)
        {
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.UserName.Equals(request.Username),
                include: x => x.Include(x => x.CurrentStageNavigation)
                );
            if (account != null) throw new BadHttpRequestException(MessageConstant.Account.AccountExisted);

            account = new Account()
            {
                UserName = request.Username,
                Password = PasswordUtil.HashPassword(request.Password),
                FullName = request.Name,
                Role = EnumUtil.GetDescriptionFromEnum(request.Role),
                Status = AccountStatus.Activate.GetDescriptionFromEnum(),
            };
            
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Account.CreateAccountFailed);
            return new GetAccountsResponse(account.Id, account.UserName, account.FullName,
                request.Role, account.CurrentStageNavigation?.Name, EnumUtil.ParseEnum<AccountStatus>(account.Status));
        }

        private Expression<Func<Account, bool>> BuildGetAccountsQuery(AccountFilter filter)
        {
            Expression<Func<Account, bool>> filterQuery = x => true;

            var searchUsername = filter.username;
            var role = filter.role;
            var stageId = filter.stageId;
            var status = filter.status;

            if (!string.IsNullOrEmpty(searchUsername))
            {
                filterQuery = filterQuery.AndAlso(x => x.UserName.Contains(searchUsername));
            }

            if (role != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Role.Equals(role.GetDescriptionFromEnum()));
            }

            if(stageId.HasValue)
            {
                filterQuery = filterQuery.AndAlso(x => x.CurrentStage.Equals(stageId));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }


        public async Task<IPaginate<GetAccountsResponse>> GetAccounts(AccountFilter filter, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;

            IPaginate<GetAccountsResponse> accounts = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                selector: x => new GetAccountsResponse(x.Id, x.UserName, x.FullName, EnumUtil.ParseEnum<RoleEnum>(x.Role), x.CurrentStageNavigation.Name, 
                    EnumUtil.ParseEnum<AccountStatus>(x.Status)),
                predicate: BuildGetAccountsQuery(filter),
                orderBy: x => x.OrderBy(x => x.UserName),
                page: page,
                size: size
                );
            return accounts;
        }

        public async Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account updateAccount = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(id));
            if (updateAccount == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            updateAccount.FullName = string.IsNullOrEmpty(updateAccountRequest.FullName) ? updateAccount.FullName : updateAccountRequest.FullName.Trim();
            updateAccount.Password = string.IsNullOrEmpty(updateAccountRequest.Password) ? updateAccount.Password : PasswordUtil.HashPassword(updateAccountRequest.Password.Trim());
            updateAccount.Status = EnumUtil.GetDescriptionFromEnum(updateAccountRequest.Status);

            _unitOfWork.GetRepository<Account>().UpdateAsync(updateAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<GetAccountsResponse> GetAccountDetail(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(x => x.CurrentStageNavigation)
                );
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);
            return new GetAccountsResponse(account.Id, account.UserName, account.FullName, 
                EnumUtil.ParseEnum<RoleEnum>(account.Role), account.CurrentStageNavigation?.Name, 
                EnumUtil.ParseEnum<AccountStatus>(account.Status));
        }

        public async Task<DentalAccountResponse> GetDentalByAccountId(int accountId)
        {
            if (accountId < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(accountId) && x.Role.Equals(RoleEnum.Dental.GetDescriptionFromEnum())
                );
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            Dental dental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.AccountId.Equals(account.Id)
                );
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.AccountDentalNotFoundMessage);

            return new DentalAccountResponse(dental.Id, dental.Name, dental.Address, 
                account.UserName, EnumUtil.ParseEnum<AccountStatus>(account.Status));
        }

        public async Task<bool> UpdateAccountStatus(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            account.Status = AccountStatus.Deactivate.GetDescriptionFromEnum();

            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            bool isSuccessfull = await _unitOfWork.CommitAsync() > 0;
            return isSuccessfull;
            
        }

        public async Task<bool> UpdateStageStaff(int accountId, UpdateStageStaffRequest request)
        {
            if (accountId < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(accountId));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            if (!account.Role.Equals(RoleEnum.Staff.GetDescriptionFromEnum()))
            {
                throw new BadHttpRequestException(MessageConstant.Account.NotStaffMessage);
            }

            ProductionStage stage = await _unitOfWork.GetRepository<ProductionStage>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(request.currentStage));
            if (stage == null) throw new BadHttpRequestException(MessageConstant.ProductionStage.NotFoundMessage);

            account.CurrentStage = request.currentStage;
            _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            bool isSuccessfull = await _unitOfWork.CommitAsync() > 0;
            return isSuccessfull;

        }
    }
}
