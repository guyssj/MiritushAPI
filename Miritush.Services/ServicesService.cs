using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class ServicesService : IServicesService
    {

        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public ServicesService(
            booksDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<List<DTO.Service>> GetServicesAsync()
        {
            var services = await dbContext.Services.ToListAsync();

            return mapper.Map<List<DTO.Service>>(services);
        }
        public async Task<DTO.Service> CreateServiceAsync(string serviceName)
        {
            var service = new Service
            {
                ServiceName = serviceName
            };
            dbContext.Services.Add(service);

            await dbContext.SaveChangesAsync();

            return mapper.Map<DTO.Service>(service);
        }
        public async Task<DTO.Service> GetServiceById(int id)
        {
            var service = await dbContext.Services.FindAsync(id);

            return mapper.Map<DTO.Service>(service);
        }
    }
}