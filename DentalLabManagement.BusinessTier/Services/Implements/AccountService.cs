using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
           
            var token = JwtUtil.GenerateJwtToken();
            LoginResponse loginResponse = new LoginResponse()
            {
                AccessToken= token,
                Id = account.AccountId,
                Username = account.UserName,
                Role = account.Role,
            };
            return loginResponse;
        }

        public async Task<Account?> CreateNewAccount(CreateNewAccountRequest createNewAccountRequest)
        {
            Account account = await _unitOfWork.GetRepository<Account>()
                .SingleOrDefaultAsync(predicate: x => x.UserName.Equals(createNewAccountRequest.Username));
            if (account != null)
            {
                throw new HttpRequestException("Account is already exist");
            }
            Account newAccount = new Account()
            {
                UserName = createNewAccountRequest.Username,
                Password = PasswordUtil.HashPassword(createNewAccountRequest.Password),
                FullName = createNewAccountRequest.Name,
                Role = createNewAccountRequest.Role,
                PhoneNumber = createNewAccountRequest.PhoneNumber,
            };
            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (isSuccessful)
            {
                return newAccount;
            }
            return null;
        }

        public async Task<IPaginate<GetAccountsResponse>> GetAccounts(string? searchUsername, int page, int size)
        {
            searchUsername = searchUsername?.Trim().ToLower();
            IPaginate<GetAccountsResponse> accounts = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(
                selector: x => new GetAccountsResponse(x.AccountId, x.UserName, x.FullName, x.Role, x.PhoneNumber),
                predicate: string.IsNullOrEmpty(searchUsername) ? x => true : x => x.UserName.ToLower().Contains(searchUsername),
                page: page,
                size: size
                );
            return accounts;
        }
    }
}
