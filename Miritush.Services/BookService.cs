using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.DTO.Const;
using Miritush.Services.Abstract;
using Miritush.Services.Helpers;

namespace Miritush.Services
{
    public class BookService : IBookService
    {
        private readonly booksDbContext dbContext;
        private readonly IUserContextService userContext;
        private readonly IMapper mapper;
        private readonly ISettingsService settingsService;
        private readonly IHttpClientFactory clientFactory;

        public BookService(
            booksDbContext dbContext,
            IUserContextService userContext,
            IMapper mapper,
            ISettingsService settingsService,
            IHttpClientFactory clientFactory)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.mapper = mapper;
            this.settingsService = settingsService;
            this.clientFactory = clientFactory;
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
            int customerId,
            int startAt,
            int serviceTypeId)
        {
            if (customerId == 0)
                throw new Exception(); //[GG] Change to exption handler

            if (await dbContext.Books.AnyAsync(book =>
                  book.StartDate == startDate
                  && book.StartAt == startAt))
                throw new Exception(); //[GG] Change to exption handler

            var serviceTypeQuery = dbContext.Servicetypes
                .Where(se => se.ServiceTypeId == serviceTypeId);

            var serviceId = await serviceTypeQuery
                .Select(st => st.ServiceId)
                .FirstOrDefaultAsync();

            if (serviceId <= 0)
                throw new ArgumentNullException("your servicetypeId not exist"); //Exeption Manager

            var duration = await serviceTypeQuery
                .Select(st => st.Duration)
                .FirstOrDefaultAsync();

            var book = new DAL.Model.Book()
            {
                CustomerId = customerId,
                StartDate = startDate,
                StartAt = startAt,
                ServiceId = serviceId,
                ServiceTypeId = serviceTypeId,
                Durtion = duration.GetValueOrDefault()
            };
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();

            if (await settingsService.GetValue(SettingsNames.SEND_SMS_APP) == "1")
                await SendBookConfirm(book.BookId);


            //push notfication to app
        }

        public async Task UpdateBookAsync(
            int bookId,
            DateTime startDate,
            int startAt,
            int customerId,
            string notes)
        {
            var book = await dbContext.Books.FindAsync(bookId);

            if (book == null)
                throw new Exception("id not exist");

            await UpdateBook(
                book.BookId,
                startDate,
                startAt,
                book.ServiceId,
                customerId,
                book.ServiceTypeId,
                notes,
                book.Durtion);

        }

        private async Task UpdateBook(
            int bookId,
            DateTime startDate,
            int startAt,
            int serviceId,
            int customerId,
            int serviceTypeId,
            string notes,
            int duration)
        {
            var book = await dbContext.Books.FindAsync(bookId);

            book.StartDate = startDate;
            book.StartAt = startAt;
            book.ServiceId = serviceId;
            book.CustomerId = customerId;
            book.ServiceTypeId = serviceTypeId;
            book.Notes = notes;
            book.Durtion = duration;

            dbContext.Books.Update(book);

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteBook(int bookId)
        {
            var book = await dbContext.Books.FindAsync(bookId);
            if (book == null)
                throw new Exception("book not exsits"); //[GG] change to exeption Handler

            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();

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

        private async Task<bool> SendBookConfirm(int bookId)
        {
            var newBook = await dbContext.Books
                .Include(cus => cus.Customer)
                .Include(serv => serv.ServiceType)
                .Where(b => b.BookId == bookId)
                .FirstOrDefaultAsync();

            var parameters = new Dictionary<string, string>();
            var messageTemplate = await settingsService.GetValue(SettingsNames.SMS_TEMPLATE_APP);
            parameters.Add("{FirstName}", newBook.Customer.FirstName);
            parameters.Add("{LastName}", newBook.Customer.LastName);
            parameters.Add("{Date}", newBook.StartDate.ToShortDateString());
            parameters.Add("{Time}", newBook.StartDate.Date.AddMinutes(newBook.StartAt).ToShortTimeString());
            parameters.Add("{ServiceType}", newBook.ServiceType.ServiceTypeName);
            parameters.Add("\\n", System.Environment.NewLine);


            var message = MessageHelper.ReplacePlaceHolder(messageTemplate, parameters);
            //send sms or not
            var results = await (await clientFactory
                .GetGlobalSmsSenderClient()
                .WithUri()
                .WithSender("Miritush")
                .ToPhoneNumber(newBook.Customer.PhoneNumber)
                .Message(message)
                .GetAsync())
                .AssertResultAsync<DTO.GlobalSmsResult>();

            return results.Success;
        }

    }
}