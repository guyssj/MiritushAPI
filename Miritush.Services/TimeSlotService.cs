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

        public async Task< List<TimeSlot>> GetSlotsExistsAsync(DateTime date)
        {
            var workHour = await dbContext.Workhours.FindAsync(date.DayOfWeek);
            var timeSlotList = new List<TimeSlot>();
            for (int i = workHour.OpenTime; i <= workHour.CloseTime; i+=30)
            {
                var timeSlot = new TimeSlot()
                {
                    Id = i,
                    Time = TimeSpan.FromMinutes(i).ToString(@"hh\:mm"),
                };
               timeSlotList.Add(timeSlot);
            }

            var books = await dbContext.Books.Where(x => x.StartDate == date).ToListAsync();
            var exsitSlots = new List<int>();
            books.ForEach(x =>
            {
                for (int i = x.StartAt; i < x.StartAt + x.Durtion; i+=5)
                {
                    exsitSlots.Add(i);
                }
            });

            return timeSlotList;
        }

    }
}
