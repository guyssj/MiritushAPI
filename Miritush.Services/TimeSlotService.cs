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
    public class TimeSlotService : ITimeSlotService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public TimeSlotService(booksDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<TimeSlot>> GetSlotsExistsAsync(DateTime date, int duration = 0)
        {
            var workHour = await dbContext.Workhours.FindAsync(((int)date.DayOfWeek));
            var timeSlotList = new List<TimeSlot>();

            var MIN_AFTER_WORK = await dbContext.Settings.FindAsync("MIN_AFTER_WORK");
            var minAfterWork = int.Parse(MIN_AFTER_WORK.SettingValue);
            for (int i = workHour.OpenTime; i <= workHour.CloseTime; i += 30)
            {
                var timeSlot = new TimeSlot()
                {
                    Id = i,
                    Time = TimeSpan.FromMinutes(i).ToString(@"hh\:mm"),
                };
                timeSlotList.Add(timeSlot);
            }

            var books = await dbContext.Books
                .Where(x => x.StartDate.Date == date.Date)
                .ToListAsync();

            var LockHours = await dbContext.Lockhours
                .Where(x => x.StartDate.Date == date.Date)
                .ToListAsync();

            var existSlots = new List<int>();
            books.ForEach(x =>
            {
                for (int i = x.StartAt; i < x.StartAt + x.Durtion; i += 5)
                {
                    existSlots.Add(i);
                    if (i == x.StartAt + x.Durtion - 5)
                    {
                        timeSlotList.Add(new TimeSlot()
                        {
                            Id = i + 5,
                            Time = TimeSpan.FromMinutes(i + 5).ToString(@"hh\:mm")
                        });
                    }
                }

            });

            LockHours.ForEach(x =>
            {
                for (int i = x.StartAt; i < x.EndAt; i += 5)
                {
                    existSlots.Add(i);
                }
            });

            existSlots = existSlots
                 .Distinct()
                 .OrderBy(x => x)
                 .ToList();

            var filterd = timeSlotList
                .Where(x => !existSlots.Any(y => y == x.Id))
                    .Where(x => !existSlots.Any(j => j > x.Id && j < (x.Id + duration)))
                .Where(x => workHour.CloseTime + minAfterWork > (x.Id + duration))
                .ToList();

            return filterd
                .OrderBy(x => x.Id)
                .ToList();
        }

    }
}
