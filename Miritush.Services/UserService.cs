using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miritush.DTO;
using Microsoft.Extensions.Configuration;
using Miritush.Services.Helpers;
using Miritush.DTO.Const;
using Miritush.DTO.Enums;
using System.Net.Http;

namespace Miritush.Services
{
    public class UserService : IUserService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserContextService userContext;
        private readonly IHttpClientFactory httpClientFactory;

        public UserService(
            booksDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IUserContextService userContext,
            IHttpClientFactory httpClientFactory)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = configuration;
            this.userContext = userContext;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<DTO.User> GetAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
                return null; //[GG] change to exption handler
            return mapper.Map<DTO.User>(user);
        }
        public async Task<DTO.User> Create(
            string userName,
            string password,
            string regId = "")
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException($"'{nameof(userName)}' cannot be null or empty.", nameof(userName));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var passwordHasher = new PasswordHasher<DAL.Model.User>();
            var user = new DAL.Model.User()
            {
                UserName = userName,
                Password = password,
                RegId = regId
            };

            user.Password = passwordHasher.HashPassword(user, password);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return mapper.Map<DTO.User>(user);

        }

        public async Task SetRegId(string regId)
        {
            if (string.IsNullOrWhiteSpace(regId))
                throw new ArgumentException($"'{nameof(regId)}' cannot be null or whitespace.", nameof(regId));

            var user = await dbContext.Users.FindAsync(userContext.Identity.UserId);

            user.RegId = regId;
            await dbContext.SaveChangesAsync();
        }
        public async Task<string> GetRegId(string regId)
        {
            if (string.IsNullOrWhiteSpace(regId))
                throw new ArgumentException($"'{nameof(regId)}' cannot be null or whitespace.", nameof(regId));

            var user = await dbContext.Users.FindAsync(userContext.Identity.UserId);
            return user.RegId;
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
        public async Task<DTO.AuthResult> Login(
            string userName,
            string password,
            GrantType grantType,
            string phoneNumber,
            int optCode)
        {
            switch (grantType)
            {
                case GrantType.authorization_code:
                    break;
                case GrantType.password:
                    if (await VerifyUserPasswordAsync(userName, password))
                        return null;
                    return JWTHelper.CreateToken(
                        configuration["Auth:Secret"],
                        configuration["Auth:ValidIssuer"],
                        userName,
                        UserRoles.Admin);
                case GrantType.passwordless_otp:
                    var customer = await AssertCustomerFromOtpAsync(phoneNumber, optCode);
                    return JWTHelper.CreateToken(
                        configuration["Auth:Secret"],
                        configuration["Auth:ValidIssuer"],
                        phoneNumber,
                        UserRoles.User);
                case GrantType.refresh_token:
                    break;
                default:
                    break;
            }
            return null;
        }

        private async Task<DTO.Customer> AssertCustomerFromOtpAsync(
            string phoneNumber,
            int optCode)
        {
            var customer = await dbContext.Customers
                .Where(cus => cus.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();

            if (customer.Otp != optCode)
                return null; //[GG] Return expeption

            return mapper.Map<DTO.Customer>(customer);

        }

        public async Task CreateOtpToCustomer(string phoneNumber)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var code = new Random()
                .Next(11111, 99999);

            var customer = await dbContext.Customers
                .Where(cus => cus.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();

            if (customer == null)
                throw new Exception("customer not found"); //[GG] Change to expection handler

            customer.Otp = code;
            await dbContext.SaveChangesAsync();

            var smsResults = await (await httpClientFactory
                .GetGlobalSmsSenderClient()
                .WithUri()
                .WithSender("Miritush")
                .ToPhoneNumber(phoneNumber)
                .Message(" זה קוד האימות שלך" + code)
                .GetAsync())
                .AssertResultAsync<GlobalSmsResult>();

            if (!smsResults.Success)
                throw new Exception(); //[GG] change to exception handler
        }
    }
}
