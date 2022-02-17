using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.DTO;
using Miritush.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Miritush.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ITimeSlotService timeSlotService;
        private readonly IHttpClientFactory clientFactory;

        public CalendarService(
            booksDbContext dbContext,
            IMapper mapper,
            ITimeSlotService timeSlotService,
            IHttpClientFactory _clientFactory)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.timeSlotService = timeSlotService;
            clientFactory = _clientFactory;
        }

        public async Task<bool> isCloseDayAsync(DateTime date)
        {
            var closeDay = await dbContext.Closedays
                .Where(x => x.Date == date)
                .FirstOrDefaultAsync();

            return closeDay != null;
        }

        public async Task<List<CloseDay>> GetCloseDayAndHolidayAsync()
        {
            var closedays = await GetCloseDaysAsync();

            var holidays = await dbContext.Holidays
                .Where(h => h.Date >= DateTime.UtcNow.Date)
                .ToListAsync();

            var filterClose = closedays
                .Where(c => holidays
                    .All(h => h.Date != c.Date))
                .ToList();

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
        private async Task<List<DTO.CloseDay>> GetCloseDaysAsync()
        {
            var closeDays = await dbContext.Closedays
                .Where(c => c.Date >= DateTime.UtcNow.Date)
                .ToListAsync();
            return mapper.Map<List<CloseDay>>(closeDays);
        }

        public async Task UpdateHolidays()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://www.hebcal.com/hebcal?v=1&cfg=json&maj=on&min=on&mod=off&nx=off&year=now&month=x&ss=off&mf=off&c=off&geo=geoname&geonameid=3448439&s=off&lg=h");

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new JsonStringEnumConverter());

            HebCalResult hebCalResult;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString()); //[GG] need to change ExepctionHandler
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var holidays = await dbContext.Holidays
                .Where(h => h.Date.Date >= DateTime.UtcNow.Date)
                .ToListAsync();
            hebCalResult = JsonSerializer.Deserialize<HebCalResult>(responseJson, options: options);
            var resHolidays = hebCalResult.items
                .Where(item => 
                       item.category == "holiday" 
                       && item.yomtov == true 
                       && item.date.Date >= DateTime.UtcNow.Date)
                .Where(item => holidays.All(h=> h.Date != item.date.Date))
                .ToList();

            if (!resHolidays.Any())
                throw new Exception(HttpStatusCode.NoContent.ToString()); //[GG] need to change ExepctionHandler

            foreach (var holiday in resHolidays)
            {
                var holidayDB = new Holiday()
                {
                    Date = holiday.date,
                    Notes = holiday.hebrew
                };
                dbContext.Holidays.Add(holidayDB);
            }
            await dbContext.SaveChangesAsync();

        }
    }
}
