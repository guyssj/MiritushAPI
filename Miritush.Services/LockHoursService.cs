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

        /// <summary>
        /// Get list Lockhours
        /// </summary>
        /// <returns></returns>
        public async Task<List<CalendarEvent<LockHour>>> List()
        {
            var lockHours = await dbContext.Lockhours
                .AsNoTracking()
                .Where(x => x.StartDate > DateTime.UtcNow.AddMonths(-3))
                .ToListAsync();

            var events = mapper.Map<List<CalendarEvent<LockHour>>>(lockHours);
            return events;
        }
        /// <summary>
        /// Get Lock hour by start date
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public async Task<List<DTO.LockHour>> GetByDate(DateTime startDate)
        {
            var lockHours = await dbContext.Lockhours
                .AsNoTracking()
                .Where(x => x.StartDate.Date == startDate.Date)
                .ToListAsync();

            return mapper.Map<List<DTO.LockHour>>(lockHours);
        }

        /// <summary>
        /// Create a Lcokhour
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
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
                .AsNoTracking()
                .Where(x => x.StartDate.Date == date.Date)
                .ToListAsync();

            foreach (var lockHour in lockHours)
            {
                for (int i = lockHour.StartAt; i < lockHour.EndAt; i += 5)
                {
                    slots.Add(i);
                }
            }

            return slots;
        }
    }
}