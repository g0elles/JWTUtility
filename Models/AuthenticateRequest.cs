using System.ComponentModel.DataAnnotations;
namespace JWTUtility.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string username {get; set;}

        [Required]
        public string password {get; set;}
    }
}