using Miritush.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface IAttachmentsService
    {
        Task AddAttachmentAsync(string attachmentName, string mimetype, int customerId);
        Task DeleteAttachmentAsync(int id);
        Task<Attachment> GetAttachmentByIdAsync(int id);
        Task<List<Attachment>> GetAttachmentsAsync();
        Task<List<Attachment>> GetAttachmentsByCustomerIdAsync(int customerId);
    }
}
