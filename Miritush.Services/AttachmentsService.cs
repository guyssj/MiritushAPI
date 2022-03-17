using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.Services
{
    public class AttachmentsService : IAttachmentsService
    {
        private readonly booksDbContext dbContext;

        public AttachmentsService(booksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Attachment>> GetAttachmentsAsync()
        {
            return await dbContext.Attachments.ToListAsync();
        }

        public async Task<Attachment> GetAttachmentByIdAsync(int id)
        {
            return await dbContext.Attachments.FindAsync(id);
        }
        public async Task AddAttachmentAsync(string attachmentName,string mimetype,int customerId)
        {
            var attachment = new Attachment
            {
                AttachmentName = attachmentName,
                MimeType = mimetype,
                CustomerId = customerId
            };
            dbContext.Attachments.Add(attachment);

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAttachmentAsync(int id)
        {
            var attachment = dbContext.Attachments.Find(id);
            if (attachment == null)
                throw new Exception("Attachment not exits");

            dbContext.Attachments.Remove(attachment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Attachment>> GetAttachmentsByCustomerIdAsync(int customerId)
        {
            return await dbContext.Attachments
                .Where(att => att.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
