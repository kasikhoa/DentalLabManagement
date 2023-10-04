using System.ComponentModel.DataAnnotations;

namespace DentalLabManagement.BusinessTier.Payload.Login;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Username's max length is 50 characters")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(64, ErrorMessage = "Password's max length is 64 characters")]
    public string Password { get; set; }
}