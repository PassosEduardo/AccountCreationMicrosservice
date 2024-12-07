using AccountService.Requests;
using AccountService.Services.Account;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers;

[ApiController]
[Route("v1/accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CreateAccountAsync([FromBody] CreateAccountRequest request)
    {
        var result = await _accountService.CreateAccountAsync(request);

        if(result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok(result.Value);      
    }

    [HttpGet("{accountId}/{confirmationToken}")]
    public async Task<ActionResult<bool>> ConfirmEmailAsync([FromRoute] string accountId, [FromRoute] string confirmationToken)
    {
        var result = await _accountService.ConfirmEmailAsync(accountId, confirmationToken);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPut("{email}/confirmation-token")]
    public async Task<ActionResult<bool>> ReSendEmailConfirmationToken([FromRoute] string email)
    {
        var result = await _accountService.ReSendEmailConfirmationToken(email);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
