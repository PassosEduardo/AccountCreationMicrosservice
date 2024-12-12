namespace EventConsumer.Entities;

public class ResetPasswordEvent
{
    public string Email { get; set; }
    public string PasswordResetKey { get; set; }
}
