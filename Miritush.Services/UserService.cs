using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miritush.DTO;
using Microsoft.Extensions.Configuration;
using Miritush.Services.Helpers;
using Miritush.DTO.Const;

namespace Miritush.Services
{
    public class UserService : IUserService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public UserService(
            booksDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<DTO.User> GetAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
                return null; //[GG] change to exption handler
            return mapper.Map<DTO.User>(user);
        }
        public async Task<bool> VerifyUserPasswordAsync(string userName, string password)
        {
            if (userName == null || password == null)
                throw new ArgumentNullException(nameof(userName));

            var user = await dbContext.Users
                .Where(x => x.UserName == userName)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("user not exsit");

            var passwordHasher = new PasswordHasher<DAL.Model.User>();
            var res = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return res == PasswordVerificationResult.Success;
        }
        public async Task<DTO.AuthResult> Login(string userName, string password)
        {

            if (!await VerifyUserPasswordAsync(userName, password))
                return null;

            return JWTHelper.CreateToken(
                configuration["Auth:Secret"],
                configuration["Auth:ValidIssuer"],
                userName,
                UserRoles.Admin);
        }

    }
}
