using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Authentication;
using Miritush.API.Model;
using System.Threading.Tasks;

namespace Miritush.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginData data)
        {
            var user = userManager.FindByIdAsync(data.UserName);

            ApplicationUser applicationUser = new ApplicationUser();
        }
    }
}
