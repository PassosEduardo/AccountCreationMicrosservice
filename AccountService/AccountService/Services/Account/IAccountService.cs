using AccountService.Entities;
using AccountService.Requests;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services.Account;

public interface IAccountService
{
    Task<Result<bool>> CreateAccountAsync(CreateAccountRequest request);
    Task<Result<bool>> ConfirmEmailAsync(string accountId, string confirmationToken);
    Task<Result<bool>> ReSendEmailConfirmationTokenAsync(string email);
    Task<Result<bool>> SendEmailForPassowordResetAsync(string email);
    Task<Result<bool>> ResetPasswordAsync(PasswordResetRequest request);
}
