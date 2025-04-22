using System.Security.Claims;

namespace NetRefreshTokenDemo.Api.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims); //ستقبل مجموعة
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken); //(هو الكائن اللي يحتوي معلومات المستخدم من التوكن
    }
}