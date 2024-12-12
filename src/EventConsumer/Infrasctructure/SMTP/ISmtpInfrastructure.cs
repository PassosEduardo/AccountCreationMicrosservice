using EventConsumer.Entities;

namespace EventConsumer.Infrasctructure.SMTP;

public interface ISmtpInfrastructure
{
    string Port { get; }
    string Host { get; }
    string Password { get; }
    string From { get; set;  }

    Task SendEmailConfirmationAsync(ConfirmEmailEvent account);
    Task SendPasswordResetAsync(ResetPasswordEvent eventValue);
}
