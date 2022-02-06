using AutoMapper;
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
    public class CalendarService : ICalendarService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public CalendarService(booksDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<DTO.CloseDay> GetCloseDaysAsync()
        {
            var closeDays = await dbContext.Closedays.ToListAsync();
            return mapper.Map<DTO.CloseDay>(closeDays);
        }
        public async Task<bool> isCloseDayAsync(DateTime date) 
        {
            var closeDay = await dbContext.Closedays
                .Where(x => x.Date == date)
                .FirstOrDefaultAsync();

            return closeDay != null;
        }

        public async Task<List<Closeday>> GetCloseDayAndHoliday()
        {
            return null;
        }
        public async Task<List<string>> GetFreeDaysAsync(
            DateTime startDate,
            int duration)
        {
            return null;
        }

        public async Task CreateCloseDay(
            DateTime date,
            string notes)
        {
            var closeDay = new Closeday()
            {
                Date = date,
                Notes = notes
            };
            dbContext.Closedays.Add(closeDay);
            await dbContext.SaveChangesAsync();

        }

        public async Task DeleteCloseDay(int id)
        {
            var closeDay = await dbContext.Closedays.FindAsync(id);
            dbContext.Closedays.Remove(closeDay);

            await dbContext.SaveChangesAsync();
        }
    }
}
