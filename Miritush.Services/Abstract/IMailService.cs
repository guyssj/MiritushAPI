using System.Threading.Tasks;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(MailRequest request);
        Task<bool> SendSimpleEmailAsync(string toEmail, string subject, string body);
    }
}