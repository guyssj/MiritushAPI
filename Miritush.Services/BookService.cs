using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class BookService : IBookService
    {
        private readonly booksDbContext dbContext;
        private readonly IUserContextService userContext;
        private readonly IMapper mapper;

        public BookService(
            booksDbContext dbContext,
            IUserContextService userContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.mapper = mapper;
        }

        public async Task<DTO.Book> GetBooksAsync()
        {
            var books = await dbContext.Books.ToListAsync();
            return mapper.Map<DTO.Book>(books);
        }

        public async Task<List<DTO.CalendarEvent<DTO.Book>>> GetBooksForCalendar()
        {
            var books = await dbContext.Books
                .Where(x => x.StartDate > DateTime.UtcNow.AddMonths(-3))
                .Include(cs => cs.Customer)
                .Include(srvt => srvt.ServiceType)
                .ToListAsync();

            var events = new List<DTO.CalendarEvent<DTO.Book>>();

            books.ForEach(book =>
            {
                var ev = new DTO.CalendarEvent<DTO.Book>()
                {
                    AllDay = false,
                    Customer = mapper.Map<DTO.Customer>(book.Customer),
                    EndTime = book.StartDate.AddMinutes(book.StartAt + book.Durtion),
                    Meta = mapper.Map<DTO.Book>(book),
                    ServiceType = mapper.Map<DTO.ServiceType>(book.ServiceType),
                    StartTime = book.StartDate.AddMinutes(book.StartAt),
                    Title = $"{book.Customer.FirstName} {book.Customer.LastName} - {book.ServiceType.ServiceTypeName}"
                };
                events.Add(ev);

            });
            return events;
        }

        public async Task SetBookAsync(
            DateTime startDate,
            int startAt,
            int serviceTypeId,
            int duration)
        {
            if (userContext.Identity.UserId == 0)
                throw new Exception(); //[GG] Change to exption handler

            if (await dbContext.Books.AnyAsync(book =>
                  book.StartDate == startDate
                  && book.StartAt == startAt))
                throw new Exception(); //[GG] Change to exption handler

            var serviceId = await dbContext.Servicetypes
                .Where(se => se.ServiceTypeId == serviceTypeId)
                .Select(srv => srv.ServiceId)
                .FirstOrDefaultAsync();

            var book = new Book()
            {
                CustomerId = userContext.Identity.UserId,
                StartDate = startDate,
                StartAt = startAt,
                ServiceId = serviceId,
                ServiceTypeId = serviceTypeId,
                Durtion = duration
            };
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();


            //send sms or not
            //push notfication to app
        }


        public async Task<List<DTO.Book>> GetCustomerFutureBooksAsync()
        {
            if (userContext.Identity.UserId == 0)
                throw new Exception(); //[GG] change to exeption handler
            //need to change display BookExtended with include
            return await GetBooksByCustomerIdAsync(userContext.Identity.UserId);
        }

        public async Task<List<DTO.Book>> GetBooksByCustomerIdAsync(int customerId)
        {
            var books = await dbContext.Books
                .Where(book => book.CustomerId == customerId
                        && book.StartDate.Date >= DateTime.UtcNow.Date)
                .ToListAsync();

            return mapper.Map<List<DTO.Book>>(books);
        }

        public async Task<List<DTO.Book>> GetBooksByPhoneNumberAsync(string phoneNumber)
        {
            var customerId = await dbContext.Customers
                .Where(cus => cus.PhoneNumber == phoneNumber)
                .Select(x => x.CustomerId)
                .FirstOrDefaultAsync();

            return await GetBooksByCustomerIdAsync(customerId);
        }

    }
}