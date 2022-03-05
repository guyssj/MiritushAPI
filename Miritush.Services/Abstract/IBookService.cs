using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface IBookService
    {
        Task DeleteBook(int bookId);
        Task<List<Book>> GetBooksByCustomerIdAsync(int customerId);
        Task<List<CalendarEvent<Book>>> GetBooksForCalendar();
        Task<List<Book>> GetCustomerFutureBooksAsync();
        Task SetBookAsync(
            DateTime startDate,
            int customerId,
            int startAt,
            int serviceTypeId);
        Task UpdateBookAsync(
            int bookId,
            DateTime startDate,
            int startAt,
            int customerId,
            string notes);
    }
}
