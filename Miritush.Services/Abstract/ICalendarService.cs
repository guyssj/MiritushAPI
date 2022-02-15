using Miritush.DAL.Model;
using Miritush.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ICalendarService
    {
        Task<List<CloseDay>> GetCloseDayAndHoliday();
        Task<List<DTO.CloseDay>> GetCloseDaysAsync();
        Task<ListResult<FreeSlots>> GetFreeDaysAsync(
            DateTime startDate,
            int duration,
            int pageNumber,
            int pageSize);
        Task<bool> isCloseDayAsync(DateTime date);
    }
}