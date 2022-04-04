using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface IBookService
    {
        Task DeleteBook(int bookId);
        Task<Book> GetBookByArrivalToken(Guid arrivalToken);
        Task<List<Book>> GetBooksByCustomerIdAsync(int customerId);
        Task<List<CalendarEvent<Book>>> GetBooksForCalendar();
        Task<List<Book>> GetCustomerFutureBooksAsync();
        Task<int> GetMonthBooksCountAsync();
        Task<List<DAL.Model.Book>> GetNextBooks();
        Task<int> GetTodayBookCountAsync();
        Task<List<Book>> SendRemainderBook();
        Task SetArrival(Guid arrivalToken, int arrivalConfrim);
        Task SetBookAsync(
            DateTime startDate,
            int customerId,
            int startAt,
            List<int> serviceTypeId);
        Task UpdateBookAsync(
            int bookId,
            DateTime startDate,
            int startAt,
            int customerId,
            string notes);
    }
}
