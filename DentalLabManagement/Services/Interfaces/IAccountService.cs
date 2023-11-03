using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.DataTier.Paginate;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<GetAccountsResponse> CreateNewAccount(AccountRequest request);
        Task<IPaginate<GetAccountsResponse>> GetAccounts(string? searchUsername, RoleEnum? role, int? stageId, AccountStatus? status , int page, int size);
        Task<GetAccountsResponse> GetAccountDetail(int id);
        Task<bool> UpdateAccountInformation(int id, UpdateAccountRequest updateAccountRequest);        
        Task<bool> UpdateAccountStatus(int id);
        Task<DentalAccountResponse> GetDentalByAccountId(int accountId);
        Task<bool> UpdateStageStaff(int accountId, UpdateStageStaffRequest request);
    }
}
