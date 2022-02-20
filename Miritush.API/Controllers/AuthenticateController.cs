using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Authentication;
using Miritush.API.Model;
using Miritush.Services.Abstract;
using System.Threading.Tasks;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthenticateController(
            IUserService userService)
        {
            this.userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<DTO.AuthResult> Login(LoginData data)
        {

           return await userService.Login(data.UserName, data.Password);
            
        }
    }
}
