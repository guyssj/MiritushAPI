using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Model;
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
        private readonly IUploadFileService uploadFileService;

        public MyController(
            IUserService userService,
            IUserContextService userContext,
            ICustomerService customerService,
            IBookService bookService,
            IUploadFileService uploadFileService)
        {
            this.userService = userService;
            this.userContext = userContext;
            this.customerService = customerService;
            this.bookService = bookService;
            this.uploadFileService = uploadFileService;
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
        [Authorize(Roles = UserRoles.User)]
        [HttpPost("Books")]
        public async Task<IActionResult> CreateBook(CreateBookData data)
        {
            await bookService.SetBookAsync(
                data.StartDate,
                userContext.Identity.UserId,
                data.StartAt,
                data.ServiceTypeId);

            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpPut("Books")]
        public async Task<IActionResult> UpdateBook(UpdateBookData data)
        {
            await bookService.UpdateBookAsync(
                data.BookId,
                data.StartDate,
                data.StartAt,
                userContext.Identity.UserId,
                data.Notes);

            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpPost("uploadfile")]
        public async Task<IActionResult> fileupload(IFormFile file)
        {

            await uploadFileService.UploadFile(file, userContext.Identity.UserId.ToString());

            return Ok();
        }

        [Authorize(Roles = UserRoles.User)]
        [HttpGet("files/{fileName}")]
        public async Task<object> GetFile(string fileName)
        {
            var res = await uploadFileService.GetFilesFolderAsync($"62/{fileName}");
            //return new { image = $"data:{res.MimeType};base64,{Convert.ToBase64String(res.Content)}" };
            return File(res.Content, res.MimeType);
        }
        [Authorize(Roles = UserRoles.User)]
        [HttpGet("files")]
        public async Task<string> GetFiles(string fileName)
        {
            var res = await uploadFileService.GetFolderFiles($"62/");
            return "";
            //return $"data:{res.MimeType};base64,{Convert.ToBase64String(res.Content)}";
            //return File(res, "image/jpg");
        }
        [AllowAnonymous]
        [HttpGet("createUser")]
        public async Task createuser([FromQuery] string userName, [FromQuery] string password)
        {
            await userService.Create(userName, password);
        }
    }
}