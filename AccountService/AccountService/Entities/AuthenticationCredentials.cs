namespace AccountService.Entities;

public sealed record AuthenticationCredentials
{
    public string EncryptedPassword { get; set; }
    public string EncryptedSalt { get; set; }

    public AuthenticationCredentials(string encryptedPassword, string encryptedSalt)
    {
        EncryptedPassword = encryptedPassword;
        EncryptedSalt = encryptedSalt;
    }
}
