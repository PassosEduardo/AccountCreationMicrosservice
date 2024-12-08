using EventConsumer.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Principal;

namespace EventConsumer.Infrasctructure.SMTP;

public class SmtpInfrastructure : ISmtpInfrastructure
{
    public string Port { get; set; }
    public string Host { get; set; }
    public string Password { get; set; }
    public string From { get; set; }

    public async Task SendEmailConfirmationAsync(ConfirmEmailEvent account)
    {
        try
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Account Service Mail", From));
            message.To.Add(new MailboxAddress("Name", account.Email));
            message.Subject = "Confirm your e-mail";
            message.Body = new TextPart("html")
            {
                Text = $"To confirm your e-mail <a href= https://localhost:7039/v1/accounts/{account.Id}/{account.EmailToken} > click here </a>"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Host);
                await client.AuthenticateAsync(From, Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }


        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public async Task SendPasswordResetAsync(ResetPasswordEvent eventValue)
    {
        try
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Account Service Mail", From));
            message.To.Add(new MailboxAddress("Name", eventValue.Email));
            message.Subject = "Password Reset";
            message.Body = new TextPart("html")
            {
                Text = @$"We have received a request to change your account password.
                To reset yout password <a> click here </a> and choose a new one"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Host);
                await client.AuthenticateAsync(From, Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }


        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

}
