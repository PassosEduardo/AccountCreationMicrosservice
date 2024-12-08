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

    /// <summary>
    /// Endpoint for creating an account
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<bool>> CreateAccountAsync([FromBody] CreateAccountRequest request)
    {
        var result = await _accountService.CreateAccountAsync(request);

        if(result.IsFailed)
            return BadRequest(result.Errors);
        
        return Ok(result.Value);      
    }

    /// <summary>
    /// Endpoint for confirming the email of account.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("{accountId}/{confirmationToken}")]
    public async Task<ActionResult<bool>> ConfirmEmailAsync([FromRoute] string accountId, [FromRoute] string confirmationToken)
    {
        var result = await _accountService.ConfirmEmailAsync(accountId, confirmationToken);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Endpoint to send another confirmation token of the e-mail.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{email}/confirmation-token")]
    public async Task<ActionResult<bool>> ReSendEmailConfirmationToken([FromRoute] string email)
    {
        var result = await _accountService.ReSendEmailConfirmationToken(email);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Endpoint to reset password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{email}/password-reset")]
    public async Task<ActionResult<bool>> SendEmailForPassowordReset([FromRoute] string email)
    {
        var result = await _accountService.SendEmailForPassowordReset(email);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
