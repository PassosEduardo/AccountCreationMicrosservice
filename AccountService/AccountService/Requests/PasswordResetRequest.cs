using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccountService.Requests;

public class PasswordResetRequest
{
    [JsonIgnore]
    public string? Email { get; set; }

    [JsonIgnore]
    public string? PasswordResetKey { get; set; }

    [FromBody]
    [Required(AllowEmptyStrings = false)]
    public string NewPassword { get; set; }

    [FromBody]
    [Required(AllowEmptyStrings = false)]
    [Compare("NewPassword")]
    public string ReNewPassword { get; set; }
}
