using Miritush.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface IUserService
    {
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<DTO.User> GetAsync(int id);
    }
}
