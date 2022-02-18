﻿using AutoMapper;
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
    public class UserService : IUserService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public UserService(
            booksDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<DTO.User> GetAsync(int id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
                return null; //[GG] change to exption handler
            return mapper.Map<DTO.User>(user); 
        }
        public async Task CheckPasswordAsync(string userName,string password)
        {
            if (userName == null || password == null)
                throw new ArgumentNullException(nameof(userName));

            var user = await dbContext.Users
                .Where(x => x.UserName == userName)
                .FirstOrDefaultAsync();

            user.
        }

    }
}
