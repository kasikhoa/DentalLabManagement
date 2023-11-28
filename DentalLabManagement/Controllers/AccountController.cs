using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Payload.Login;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.BusinessTier.Validators;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Dental;

namespace DentalLabManagement.API.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }
        
        [HttpPost(ApiEndPointConstant.Authentication.Login)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var loginResponse = await _accountService.Login(loginRequest);
            if (loginResponse == null) 
                throw new BadHttpRequestException(MessageConstant.LoginMessage.InvalidUsernameOrPassword);
            if (loginResponse.Status == AccountStatus.Deactivate) 
                throw new BadHttpRequestException(MessageConstant.LoginMessage.DeactivatedAccount);
            return Ok(loginResponse);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.Account.AccountsEndPoint)]
        [ProducesResponseType(typeof(GetAccountsResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateAccount(AccountRequest createNewAccountRequest)
        {
            var response = await _accountService.CreateNewAccount(createNewAccountRequest);
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.Account.AccountsEndPoint)]
        [ProducesResponseType(typeof(IPaginate<GetAccountsResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllAccount([FromQuery] AccountFilter filter, [FromQuery] int page, [FromQuery] int size)
        {
            var response = await _accountService.GetAccounts(filter, page, size);
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateAccountInformation(int id, [FromBody] UpdateAccountRequest updateAccountRequest)
        {
            var isSuccessful = await _accountService.UpdateAccountInformation(id, updateAccountRequest);
            if (!isSuccessful) return Ok(MessageConstant.Account.UpdateAccountFailedMessage);
            return Ok(MessageConstant.Account.UpdateAccountSuccessfulMessage);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesResponseType(typeof(GetAccountsResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetAccountDetail(int id)
        {
            var response = await _accountService.GetAccountDetail(id);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Account.DentalAcccountEndPoint)]
        [ProducesResponseType(typeof(DentalAccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetDentalByAccountId(int id)
        {
            var response = await _accountService.GetDentalByAccountId(id);
            return Ok(response);

        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var isSuccessful = await _accountService.DeleteAccount(id);
            if (!isSuccessful) return Ok(MessageConstant.Account.UpdateAccountStatusFailedMessage);
            return Ok(MessageConstant.Account.UpdateAccountStatusSuccessfulMessage);
        }

        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Manager)]
        [HttpPatch(ApiEndPointConstant.Account.StaffEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateStageStaff(int id, UpdateStageStaffRequest request)
        {
            var isSuccessful = await _accountService.UpdateStageStaff(id, request);
            if (!isSuccessful) return Ok(MessageConstant.Account.UpdateStageFailedMessage);
            return Ok(MessageConstant.Account.UpdateStageSuccessMessage);
        }

    }
}
