using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface IUploadFileService
    {
        Task<DTO.BlobFile> GetFilesFolderAsync(string folderName);
        Task<List<BlobFile>> GetFolderFiles(string folderName);
        Task UploadFile(IFormFile file, string folderName);
    }
}