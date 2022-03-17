using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class UploadFileService : IUploadFileService
    {
        public async Task UploadFile(IFormFile file, string folderName)
        {
            using (var client = new AmazonS3Client(
                "AKIA5DLMWGUBJGM44X6E",
                "XgSkWO9qpSLvXPA5wCOcd9HhX5/tINw9NB5p+XJK",
                RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = $"{folderName}/{file.FileName}", // filename
                        BucketName = "booknailfiles" // bucket name of S3
                    };
                    var fileTransferUtility = new TransferUtility(client);
                    if (await FileExist(uploadRequest.BucketName, uploadRequest.Key))
                        uploadRequest.Key = $"{folderName}/{Guid.NewGuid()}_{file.FileName}";
                    await fileTransferUtility.UploadAsync(uploadRequest);


                }
            }
            //await GetFilesFolderAsync(folderName);
        }

        public async Task<bool> GetFileBlobAsync(string fileName)
        {
            return true;
        }
        private byte[] ReadStream(Stream responseStream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public async Task<List<DTO.BlobFile>> GetFolderFiles(string folderName)
        {
            using (var client = new AmazonS3Client(
                "AKIA5DLMWGUBJGM44X6E",
                "XgSkWO9qpSLvXPA5wCOcd9HhX5/tINw9NB5p+XJK",
                RegionEndpoint.USEast1))
            {

                var objReq = new ListObjectsV2Request
                {
                    BucketName = "booknailfiles",
                    Prefix = folderName,    // the file name
                };
                var listobj = await client.ListObjectsV2Async(objReq);
                var fileTransferUtility = new TransferUtility(client);
                var filesList = new List<DTO.BlobFile>();
                listobj.S3Objects.ForEach(obj =>
                {
                    filesList.Add(new DTO.BlobFile { FileName = obj.Key });
                });

                foreach (var item in listobj.S3Objects)
                {
                   filesList.Add(await GetFilesFolderAsync(item.Key));
                }
                return filesList;
            }
        }
        public async Task<DTO.BlobFile> GetFilesFolderAsync(string folderFileName)
        {
            using (var client = new AmazonS3Client(
                "AKIA5DLMWGUBJGM44X6E",
                "XgSkWO9qpSLvXPA5wCOcd9HhX5/tINw9NB5p+XJK",
                RegionEndpoint.USEast1))
            {

                var objReq = new GetObjectRequest
                {
                    BucketName = "booknailfiles",
                    Key = folderFileName,    // the file name
                };
                using var objResp = await client.GetObjectAsync(objReq);
                var fileByte = await GetFileStreamToBytes(objResp.ResponseStream);

                return new DTO.BlobFile()
                {
                    FileName = folderFileName,
                    MimeType = objResp.Headers.ContentType,
                    Content = fileByte
                };
            }
        }

        private async Task<byte[]> GetFileStreamToBytes(Stream objStream)
        {
            using var ms = new MemoryStream();
            await objStream.CopyToAsync(ms);  // _ct is a CancellationToken
            return ms.ToArray();
        }

        private async Task<bool> FileExist(string bucketName, string folderFileName)
        {
            using (var client = new AmazonS3Client(
                "AKIA5DLMWGUBJGM44X6E",
                "XgSkWO9qpSLvXPA5wCOcd9HhX5/tINw9NB5p+XJK",
                RegionEndpoint.USEast1))
            {
                try
                {
                    await client.GetObjectAsync(bucketName, folderFileName);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
    }
}