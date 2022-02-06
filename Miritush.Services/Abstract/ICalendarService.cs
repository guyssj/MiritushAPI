using Miritush.DAL.Model;
using Miritush.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ICalendarService
    {
        Task<List<Closeday>> GetCloseDayAndHoliday();
        Task<CloseDay> GetCloseDaysAsync();
        Task<List<string>> GetFreeDaysAsync(DateTime startDate, int duration);
        Task<bool> isCloseDayAsync(DateTime date);
    }
}