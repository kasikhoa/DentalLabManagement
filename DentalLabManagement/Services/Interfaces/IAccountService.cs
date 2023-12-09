using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.BusinessTier.Payload.Partner;
using DentalLabManagement.DataTier.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse?> Login(LoginRequest loginRequest);
        Task<int> CreateNewAccount(AccountRequest request);
        Task<IPaginate<GetAccountsResponse>> GetAccounts(AccountFilter filter, int page, int size);
        Task<GetAccountsResponse> GetAccountDetail(int id);
        Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest);        
        Task<bool> DeleteAccount(int id);
        Task<PartnerAccountResponse> GetDentalByAccountId(int accountId);
        Task<bool> UpdateStageStaff(int accountId, UpdateStageStaffRequest request);
    }
}
