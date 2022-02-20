﻿using Miritush.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface IUserService
    {
        Task<DTO.User> GetAsync(int id);
        Task<AuthResult> Login(string userName, string password);
        Task<bool> VerifyUserPasswordAsync(string userName, string password);
    }
}
