using AccountService.Requests;
using AccountService.Services.Account;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
    /// Creates an account
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
    /// Confirms the email of account.
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
    /// Sends a new confirmation token of the e-mail.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{email}/confirmation-token")]
    public async Task<ActionResult<bool>> ReSendEmailConfirmationTokenAsync([FromRoute] string email)
    {
        var result = await _accountService.ReSendEmailConfirmationTokenAsync(email);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Sends an e-mail for password reset
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{email}/forgot-password")]
    public async Task<ActionResult<bool>> SendEmailForPassowordResetAsync([FromRoute] string email)
    {
        var result = await _accountService.SendEmailForPassowordResetAsync(email);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Resets password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{email}/password-reset")]
    public async Task<ActionResult<bool>> ResetPasswordAsync([FromBody] PasswordResetRequest request,
                                                             [FromQuery(Name = "key")] [Required] string passwordResetKey,
                                                             [FromRoute] string email)
    {
        request.Email = email;
        request.PasswordResetKey = passwordResetKey;

        var result = await _accountService.ResetPasswordAsync(request);

        if (result.IsFailed)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
