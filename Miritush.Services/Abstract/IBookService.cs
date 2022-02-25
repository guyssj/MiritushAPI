using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksByCustomerIdAsync(int customerId);
        Task<List<CalendarEvent<Book>>> GetBooksForCalendar();
        Task<List<Book>> GetCustomerFutureBooksAsync();
        Task SetBookAsync(
            DateTime startDate,
            int startAt,
            int serviceTypeId,
            int duration);
    }
}
