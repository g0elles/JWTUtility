using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JWTUtility.Entities;
using JWTUtility.Helpers;
using JWTUtility.Models;

namespace JWTUtility.Services
{

    public interface IUserService 
    {

        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);

    }

    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {   
            new User {Id = 1, FirstName = "Test", LastName="User", UserName="test",Password="test"}
        };

        public readonly Appsettings _appSettings;
        public UserService(IOptions<Appsettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.UserName == model.username && x.Password == model.password);
            if (user == null) return null;

            var token = generateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        private string generateJwtToken (User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{new Claim("id",user.Id.ToString())}),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}