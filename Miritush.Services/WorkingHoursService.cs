using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.DTO;
using Miritush.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.Services
{
    public class WorkingHoursService : IWorkingHoursService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public WorkingHoursService(booksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<WorkHour>> ListAsync()
        {
            var workingHours = await dbContext.Workhours.ToListAsync();

            return mapper.Map<List<WorkHour>>(workingHours);
        }
        public async Task<WorkHour> GetWorkHourByDateAsync(int dayofWeek)
        {
            var workHour = await dbContext.Workhours.FindAsync(dayofWeek);

            return mapper.Map<WorkHour>(workHour);
        }
    }
}
