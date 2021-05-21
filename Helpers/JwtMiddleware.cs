using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWTUtility.Services;

namespace JWTUtility.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Appsettings _appSettings;

        public JwtMiddleware (RequestDelegate next, IOptions<Appsettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                },out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userID = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);

            context.Items["User"] = userService.GetById(userID);
            }
            catch (System.Exception)
            {}
        }
        
    }
}