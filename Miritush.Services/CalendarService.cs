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
    public class CalendarService : ICalendarService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ITimeSlotService timeSlotService;

        public CalendarService(
            booksDbContext dbContext,
            IMapper mapper,
            ITimeSlotService timeSlotService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.timeSlotService = timeSlotService;
        }

        public async Task<List<DTO.CloseDay>> GetCloseDaysAsync()
        {
            var closeDays = await dbContext.Closedays.ToListAsync();
            return mapper.Map<List<DTO.CloseDay>>(closeDays);
        }
        public async Task<bool> isCloseDayAsync(DateTime date)
        {
            var closeDay = await dbContext.Closedays
                .Where(x => x.Date == date)
                .FirstOrDefaultAsync();

            return closeDay != null;
        }

        public async Task<List<CloseDay>> GetCloseDayAndHoliday()
        {
            var closedays = await GetCloseDaysAsync();

            var holidays = await dbContext.Holidays.ToListAsync();
            var filterClose = closedays.Where(c => holidays.All(h => h.Date != h.Date)).ToList();

            Random rnd = new Random();
            holidays.ForEach(ho =>
            {

                filterClose.Add(
                    new CloseDay
                    {
                        Id = ho.HolidayId + rnd.Next(0, 9999),
                        Date = ho.Date,
                        Notes = ho.Notes
                    });
            });
            return filterClose
                .Distinct()
                .OrderBy(x => x.Date)
                .ToList();
        }
        public async Task<ListResult<FreeSlots>> GetFreeDaysAsync(
            DateTime startDate,
            int duration = 0,
            int pageNumber = 1,
            int pageSize = 20)
        {
            var date = DateTime.UtcNow.AddDays(31);
            var listFreeSlots = new List<FreeSlots>();
            var endDate = new DateTime(date.Year, date.Month, startDate.Day);

            foreach (var day in EachDay(startDate, endDate))
            {
                var slots = await timeSlotService.GetTimeSlotsAsync(day, duration);
                slots.ForEach(slot =>
                {
                    listFreeSlots.Add(new FreeSlots(day, slot.Id, slot.Id + duration));
                });
            }
            var results = new ListResult<FreeSlots>(pageNumber, pageSize);
            results.TotalRecord = listFreeSlots.Count;
            results.Data = listFreeSlots
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(x => x.startDate)
                .ToList();

            return results;

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
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
