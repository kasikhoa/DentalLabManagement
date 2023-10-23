using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<GetAccountsResponse> CreateNewAccount(AccountRequest createNewAccountRequest);
        Task<IPaginate<GetAccountsResponse>> GetAccounts(string? searchUsername, RoleEnum? role, AccountStatus? status , int page, int size);
        Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest);
        Task<GetAccountsResponse> GetAccountDetail(int id);
        Task<bool> UpdateAccountStatus(int id);
    }
}
