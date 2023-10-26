using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Payload.Login;
using Microsoft.AspNetCore.Authorization;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.BusinessTier.Validators;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Error;

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
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
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
        public async Task<IActionResult> ViewAllAccount([FromQuery] string? username, [FromQuery] RoleEnum? role, AccountStatus? status , [FromQuery] int page, [FromQuery] int size)
        {
            var response = await _accountService.GetAccounts(username, role, status , page, size);
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

        [HttpGet(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesResponseType(typeof(GetAccountsResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> GetAccountDetail(int id)
        {
            var response = await _accountService.GetAccountDetail(id);
            return Ok(response);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Account.AccountEndPoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateAccountStatus(int id)
        {
            var isSuccessful = await _accountService.UpdateAccountStatus(id);
            if (!isSuccessful) return Ok(MessageConstant.Account.UpdateAccountStatusFailedMessage);
            return Ok(MessageConstant.Account.UpdateAccountStatusSuccessfulMessage);
        }
    }
}
