using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IBookService bookService;
        private readonly IMapper mapper;
        private readonly IUploadFileService uploadFileService;
        private readonly IAttachmentsService attachmentsService;

        public MyController(
            IUserService userService,
            IUserContextService userContext,
            ICustomerService customerService,
            IBookService bookService,
            IMapper mapper,
            IUploadFileService uploadFileService,
            IAttachmentsService attachmentsService )
        {
            this.userService = userService;
            this.userContext = userContext;
            this.bookService = bookService;
            this.mapper = mapper;
            this.uploadFileService = uploadFileService;
            this.attachmentsService = attachmentsService;
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
                data.ServiceTypeIds);

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
            var userId = userContext.Identity.UserId;

            //add file to aws s3 blob
            await uploadFileService.UploadFile(file, userId.ToString());

            //add deatils file to attachments table
            await attachmentsService.AddAttachmentAsync(file.FileName, file.ContentType, userId);

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
        public async Task<List<Attachment>> GetFiles()
        {
            var res = await attachmentsService.GetAttachmentsByCustomerIdAsync(userContext.Identity.UserId);
            return mapper.Map<List<DTO.Attachment>>(res);
        }
        [AllowAnonymous]
        [HttpGet("createUser")]
        public async Task createuser([FromQuery] string userName, [FromQuery] string password)
        {
            await userService.Create(userName, password);
        }
    }
}