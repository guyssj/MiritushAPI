using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miritush.DTO;
using Miritush.DTO.Const;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IUploadFileService uploadFileService;
        private readonly IAttachmentsService attachmentsService;
        private readonly IMapper mapper;

        public AttachmentsController(
            IUploadFileService uploadFileService,
            IAttachmentsService attachmentsService,
            IMapper mapper)
        {
            this.uploadFileService = uploadFileService;
            this.attachmentsService = attachmentsService;
            this.mapper = mapper;
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("{customerId}/upload")]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromRoute] int customerId)
        {
            await uploadFileService.UploadFile(file, customerId.ToString());

            //add file name and type to attachments table
            await attachmentsService.AddAttachmentAsync(file.FileName, file.ContentType, customerId);

            return Ok();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("{customerId}/{fileName}")]
        public async Task<object> GetFile(
            [FromRoute] string fileName,
            [FromRoute] int customerId)
        {
            var res = await uploadFileService.GetFilesFolderAsync($"{customerId}/{fileName}");
            return File(res.Content, res.MimeType);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("{customerId}/allfiles")]
        public async Task<List<Attachment>> GetFiles([FromRoute] int customerId)
        {
            var res = await attachmentsService.GetAttachmentsByCustomerIdAsync(customerId);
            return mapper.Map<List<DTO.Attachment>>(res);
        }

    }
}