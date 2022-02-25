using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miritush.DTO;
using Miritush.DTO.Const;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserContextService userContext;
        private readonly ICustomerService customerService;
        private readonly IBookService bookService;

        public MyController(
            IUserService userService,
            IUserContextService userContext,
            ICustomerService customerService,
            IBookService bookService)
        {
            this.userService = userService;
            this.userContext = userContext;
            this.customerService = customerService;
            this.bookService = bookService;
        }

        [HttpGet("details")]
        public async Task<ProfileDetailsResult> GetUserDetails()
        {
            var user = await userService
                .GetAsync(userContext.Identity.UserId);

            if (user == null)
            {
                return new ProfileDetailsResult
                {
                    Name = $"{userContext.Identity.FirstName} {userContext.Identity.LastName}",
                    PhoneNumber = userContext.Identity.Name
                };
            }

            return new ProfileDetailsResult { Name = userContext.Identity.Name, PhoneNumber = user.RegId };

        }
        [Authorize(Roles = UserRoles.User)]
        [HttpGet("Books")]
        public async Task<List<DTO.Book>> GetMyBooks()
        {
            return await bookService
                .GetCustomerFutureBooksAsync();
        }
    }
}