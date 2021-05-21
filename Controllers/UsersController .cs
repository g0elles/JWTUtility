using Microsoft.AspNetCore.Mvc;
using JWTUtility.Models;
using JWTUtility.Services;
using Microsoft.AspNetCore.Authorization;


namespace JWTUtility.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        
        public UsersController (IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);
            
            if (response == null)
                return BadRequest(new {message = "Username or password incorrect"});

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        
    }
}