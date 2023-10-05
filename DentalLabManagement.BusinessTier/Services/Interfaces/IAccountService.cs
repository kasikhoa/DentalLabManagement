using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.BusinessTier.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<LoginResponse> Login(LoginRequest loginRequest);

        public Task<Account?> CreateNewAccount(CreateNewAccountRequest createNewAccountRequest);

        public Task<IPaginate<GetAccountsResponse>> GetAccounts(string? searchUsername, int page, int size);
    }
}
