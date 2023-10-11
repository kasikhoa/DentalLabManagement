
using DentalLabManagement.BusinessTier.Enums;

namespace DentalLabManagement.BusinessTier.Payload.Login;

public class LoginResponse
{
    public string AccessToken { get; set; }
    public int Id { get; set; }
    public string Username { get; set; }
    public RoleEnum Role { get; set; }    
    public AccountStatus Status { get; set; }

    public LoginResponse(string accessToken, int id, string username, RoleEnum role, AccountStatus status)
    {
        AccessToken = accessToken;
        Id = id;
        Username = username;
        Role = role;
        Status = status;
    }

    public LoginResponse()
    {
    }
}

