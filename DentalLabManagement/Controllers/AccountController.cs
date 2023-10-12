using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Payload.Login;
using Microsoft.AspNetCore.Authorization;
using DentalLabManagement.BusinessTier.Payload.Account;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.BusinessTier.Validators;
using DentalLabManagement.BusinessTier.Enums;

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
            {
                return Unauthorized(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Error = MessageConstant.LoginMessage.InvalidUsernameOrPassword,
                    TimeStamp = DateTime.Now
                });
            }
            if (loginResponse.Status == AccountStatus.Deactivate)
                return Unauthorized(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Error = MessageConstant.LoginMessage.DeactivatedAccount,
                    TimeStamp = DateTime.Now
                });
            return Ok(loginResponse);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> CreateAccount(AccountRequest createNewAccountRequest)
        {
            var response = await _accountService.CreateNewAccount(createNewAccountRequest);
            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok( response);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<GetAccountsResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllAccount([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
            var accounts = await _accountService.GetAccounts(name, page, size);
            return Ok(accounts);
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.Account.AccountEndpoint)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> UpdateAccountInformation(int id, [FromBody] UpdateAccountRequest updateAccountRequest)
        {
            await _accountService.UpdateAccountInformation(id, updateAccountRequest);
            return Ok(MessageConstant.Account.UpdateAccountStatusSuccessfulMessage);
        }


    }
}
