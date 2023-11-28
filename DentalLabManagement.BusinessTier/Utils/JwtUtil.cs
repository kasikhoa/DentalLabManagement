using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.DataTier.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DentalLabManagement.BusinessTier.Utils;

public class JwtUtil
{
    private JwtUtil()
    {
    }

    public static string GenerateJwtToken(Account account)
    {
        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        SymmetricSecurityKey secrectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DentalLabNumberOne"));        
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
        string issuer = "DentalLab";
        List<Claim> claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub,account.UserName),
            new Claim(ClaimTypes.Role,account.Role),
        };
        var expires = DateTime.Now.AddMonths(1);
        var token = new JwtSecurityToken(issuer, null, claims, notBefore: DateTime.Now, expires, credentials);
        return jwtHandler.WriteToken(token);
    }
}