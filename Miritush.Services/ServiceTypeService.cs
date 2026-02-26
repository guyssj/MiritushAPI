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
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public ServiceTypeService(
            booksDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<DTO.ServiceType>> List()
        {
            var serviceTypes = await dbContext.Servicetypes
                .AsNoTracking()
                .ToListAsync();

            return mapper.Map<List<DTO.ServiceType>>(serviceTypes);
        }
        public async Task<DTO.ServiceType> Get(int id)
        {
            var serviceType = await dbContext.Servicetypes
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.ServiceTypeId == id);

            return mapper.Map<DTO.ServiceType>(serviceType);
        }
        public async Task<List<DTO.ServiceType>> GetByService(int serviceId)
        {
            var serviceTypes = await dbContext.Servicetypes
                .AsNoTracking()
                .Where(x => x.ServiceId == serviceId)
                .ToListAsync();
            return mapper.Map<List<DTO.ServiceType>>(serviceTypes);
        }

        public async Task CreateServiceType(
            string serviceTypeName,
            int serviceId,
            int duration,
            decimal price,
            string description = "")
        {
            if (string.IsNullOrWhiteSpace(serviceTypeName))
                throw new ArgumentException($"'{nameof(serviceTypeName)}' cannot be null or whitespace.", nameof(serviceTypeName));
            if (serviceId == 0)
                throw new ArgumentException($"'{nameof(serviceId)}' cannot be zero", nameof(serviceId));


            var serviceType = new Servicetype()
            {
                ServiceTypeName = serviceTypeName,
                ServiceId = serviceId,
                Duration = duration,
                Price = price,
                Description = description
            };

            dbContext.Servicetypes.Add(serviceType);

            await dbContext.SaveChangesAsync();
        }
    }
}