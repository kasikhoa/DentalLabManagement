using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Implements
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<AccountService> logger ) : base(unitOfWork, logger)
        {

        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Expression<Func<Account, bool>> searchFilter = p =>
                p.UserName.Equals(loginRequest.Username) &&
                p.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));
            Account account = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: searchFilter);
            if (account == null) return null;
           
            var token = JwtUtil.GenerateJwtToken(account);
            
            LoginResponse loginResponse = new LoginResponse()
            {
                AccessToken = token,
                Id = account.AccountId,
                Username = account.UserName,
                Role = EnumUtil.ParseEnum<RoleEnum>(account.Role),
                Status = EnumUtil.ParseEnum<AccountStatus>(account.Status),
            };
            return loginResponse;
        }

        public async Task<GetAccountsResponse> CreateNewAccount(AccountRequest createNewAccountRequest)
        {
            Account account = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: x => x.UserName.Equals(createNewAccountRequest.Username));
            if (account != null)
            {
                throw new HttpRequestException(MessageConstant.Account.AccountExisted);
            }
            account = new Account()
            {
                UserName = createNewAccountRequest.Username,
                Password = PasswordUtil.HashPassword(createNewAccountRequest.Password),
                FullName = createNewAccountRequest.Name,
                Role = EnumUtil.GetDescriptionFromEnum(createNewAccountRequest.Role),
                Status = EnumUtil.GetDescriptionFromEnum(createNewAccountRequest.Status)
            };
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new HttpRequestException(MessageConstant.Account.CreateAccountFailed);
            return new GetAccountsResponse(account.AccountId, account.UserName, account.FullName,
                createNewAccountRequest.Role, createNewAccountRequest.Status);
        }

        public async Task<IPaginate<GetAccountsResponse>> GetAccounts(string? searchUsername, int page, int size)
        {
            searchUsername = searchUsername?.Trim().ToLower();
            IPaginate<GetAccountsResponse> accounts = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                selector: x => new GetAccountsResponse(x.AccountId, x.UserName, x.FullName, EnumUtil.ParseEnum<RoleEnum>(x.Role), EnumUtil.ParseEnum<AccountStatus>(x.Status)),
                predicate: string.IsNullOrEmpty(searchUsername) ? x => true : x => x.UserName.ToLower().Contains(searchUsername),
                page: page,
                size: size
                );
            return accounts;
        }

        public async Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest)
        {
            if (id < 1) throw new HttpRequestException(MessageConstant.Account.EmptyAccountId);
            Account updateAccount = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: x => x.AccountId.Equals(id));
            if (updateAccount == null)
                throw new HttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            updateAccount.FullName = string.IsNullOrEmpty(updateAccountRequest.FullName) ? updateAccount.FullName : updateAccountRequest.FullName;
            updateAccount.Password = string.IsNullOrEmpty(updateAccountRequest.Password) ? updateAccount.Password : PasswordUtil.HashPassword(updateAccountRequest.Password.Trim());
            _unitOfWork.GetRepository<Account>().UpdateAsync(updateAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
