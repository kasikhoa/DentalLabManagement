using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<LoginResponse> Login(LoginRequest loginRequest);
        public Task<GetAccountsResponse> CreateNewAccount(AccountRequest createNewAccountRequest);
        public Task<IPaginate<GetAccountsResponse>> GetAccounts(string? searchUsername, RoleEnum? role , int page, int size);
        public Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest);
        public Task<GetAccountsResponse> GetAccountDetail(int id);
    }
}
