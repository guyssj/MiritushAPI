using Miritush.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ITimeSlotService
    {
        Task<List<TimeSlot>> GetTimeSlotsAsync(DateTime date, int duration = 0);
    }
}