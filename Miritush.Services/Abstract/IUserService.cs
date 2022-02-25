using Miritush.DTO;
using Miritush.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface IUserService
    {
        Task<User> Create(string userName, string password, string regId = "");
        Task CreateOtpToCustomer(string phoneNumber);
        Task<DTO.User> GetAsync(int id);
        Task<string> GetRegId(string regId);
        Task<AuthResult> Login(
            string userName,
            string password,
            GrantType grantType,
            string phoneNumber,
            int optCode);
        Task SetRegId(string regId);
        Task<bool> VerifyUserPasswordAsync(string userName, string password);
    }
}
