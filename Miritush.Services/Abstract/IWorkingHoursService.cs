using Miritush.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface IWorkingHoursService
    {
        Task<WorkHour> GetWorkHourByDateAsync(int dayofWeek);
        Task<List<WorkHour>> ListAsync();
    }
}