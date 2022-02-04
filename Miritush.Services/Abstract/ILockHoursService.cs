using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface ILockHoursService
    {
        Task CreateLockHour(DateTime startDate, int startAt, int endAt, string notes);
        Task DeleteLockHour(int id);
        Task<List<int>> GetSlotsLockByDate(DateTime date);
        Task<List<CalendarEvent<LockHour>>> List();
    }
}