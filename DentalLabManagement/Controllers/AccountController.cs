using Microsoft.AspNetCore.Mvc;
using DentalLabManagement.API.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Payload.Login;
using Microsoft.AspNetCore.Authorization;
using DentalLabManagement.BusinessTier.Payload.Account;
using static DentalLabManagement.API.Constants.ApiEndPointConstant;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.BusinessTier.Validators;

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
     
            return Ok(loginResponse);
        }

        [Authorize]
        [HttpPost(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAccount(CreateNewAccountRequest createNewAccountRequest)
        {
            var response = await _accountService.CreateNewAccount(createNewAccountRequest);
            if (response == null)
            {
                return BadRequest(NotFound());
            }
            return Ok( response);
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<GetAccountsResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> ViewAllAccount([FromQuery] string? name, [FromQuery] int page, [FromQuery] int size)
        {
            var accounts = await _accountService.GetAccounts(name, page, size);
            return Ok(accounts);
        }

    }
}
