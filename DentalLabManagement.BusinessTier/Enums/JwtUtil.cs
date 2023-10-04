using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DentalLabManagement.BusinessTier.Enums;

public class JwtUtil
{
    private JwtUtil()
    {

    }

    public static string GenerateJwtToken()
    {

        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        SymmetricSecurityKey secrectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DentalLabNumberOne"));
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
     
        var expires =  DateTime.Now.AddDays(10);
        var token = new JwtSecurityToken("DentalLab", null, null, notBefore: DateTime.Now, expires, credentials);
        return jwtHandler.WriteToken(token);
    }
}