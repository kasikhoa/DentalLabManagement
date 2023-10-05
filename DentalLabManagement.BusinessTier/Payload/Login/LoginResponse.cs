
using DentalLabManagement.BusinessTier.Enums;

namespace DentalLabManagement.BusinessTier.Payload.Login;

public class LoginResponse
{
    public string AccessToken { get; set; }
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }

}

