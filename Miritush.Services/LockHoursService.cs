using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.DTO;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class LockHoursService : ILockHoursService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public LockHoursService(booksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<CalendarEvent<LockHour>>> List()
        {
            var lockHours = await dbContext.Lockhours
                .Where(x => x.StartDate > DateTime.UtcNow.AddMonths(-3))
                .ToListAsync();

            var events = mapper.Map<List<CalendarEvent<LockHour>>>(lockHours);
            return events;
        }
        public async Task<List<DTO.LockHour>> GetByDate(DateTime startDate)
        {
            var lockHours = await dbContext.Lockhours
                .Where(x => x.StartDate.Value == startDate)
                .ToListAsync();

            return mapper.Map<List<DTO.LockHour>>(lockHours);
        }

        public async Task CreateLockHour(
            DateTime startDate,
            int startAt,
            int endAt,
            string notes)
        {

            var lockHour = new Lockhour()
            {
                StartDate = startDate,
                StartAt = startAt,
                EndAt = endAt,
                Notes = notes
            };

            dbContext.Lockhours.Add(lockHour);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteLockHour(int id)
        {
            var lockHour = await dbContext.Lockhours.FindAsync(id);
            dbContext.Lockhours.Remove(lockHour);

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<int>> GetSlotsLockByDate(DateTime date)
        {
            var slots = new List<int>();
            var lockHours = await dbContext.Lockhours
                .Where(x => x.StartDate.Value == date)
                .ToListAsync();

            foreach (var lockHour in lockHours)
            {
                for (int i = lockHour.StartAt.Value; i < lockHour.EndAt.Value; i += 5)
                {
                    slots.Add(i);
                }
            }

            return slots;
        }
    }
}