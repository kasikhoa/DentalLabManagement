using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.DataTier.Models;

namespace DentalLabManagement.BusinessTier.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<LoginResponse> Login(LoginRequest loginRequest);

       public Task<Account?> CreateNewAccount(CreateNewAccountRequest createNewAccountRequest);
    }
}
