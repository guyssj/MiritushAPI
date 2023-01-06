using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.DTO.Const;
using Miritush.Helpers.Exceptions;
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

        private async Task<List<Book>> GetBooksByDateAsync(DateTime date)
        {
            var books = await dbContext.Books
                .Where(book => book.StartDate.Date == date.Date)
                .ToListAsync();

            return books;
        }

        private async Task<List<Book>> GetBooksByRangeDatesAsync(DateTime startDate, DateTime endDate)
        {
            //[GG] test for end date in unit tests!
            var books = await dbContext.Books
                .Where(book => book.StartDate.Date >= startDate.Date
                    && book.StartDate <= endDate.AddDays(1).Date)
                .ToListAsync();

            return books;
        }

        public async Task<int> GetTodayBookCountAsync()
        {
            var books = await GetBooksByDateAsync(DateTime.UtcNow);

            return books.Count;
        }

        public async Task<int> GetMonthBooksCountAsync()
        {
            var date = DateTime.UtcNow;
            var firstDayInMonth = date.AddDays(1 - date.Day);
            var lastDateInMonth = date.AddDays(DateTime.DaysInMonth(date.Year, date.Month) - 1);
            var books = await GetBooksByRangeDatesAsync(firstDayInMonth, lastDateInMonth);

            return books.Count;
        }

        public async Task<List<Book>> GetNextBooks()
        {
            var books = await dbContext.Books
                .Where(book => book.StartDate.Date >= DateTime.UtcNow.Date)
                .Take(5)
                .OrderBy(b => b.StartAt)
                .ToListAsync();

            return books;

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

        public async Task<List<DTO.CalendarEventApp<DTO.Book>>> GetBooksForCalendarApp()
        {
            var books = await dbContext.Books
                .Where(x => x.StartDate > DateTime.UtcNow.AddMonths(-3))
                .Include(cs => cs.Customer)
                .Include(srvt => srvt.ServiceType)
                .ToListAsync();

            var bo = books.Select(book => new DTO.CalendarEventApp<DTO.Book>
            {
                AllDay = false,
                Customer = mapper.Map<DTO.Customer>(book.Customer),
                End = book.StartDate.AddMinutes(book.StartAt + book.Durtion),
                Meta = mapper.Map<DTO.Book>(book),
                ServiceType = mapper.Map<DTO.ServiceType>(book.ServiceType),
                start = book.StartDate.AddMinutes(book.StartAt),
                Title = $"{book.Customer.FirstName} {book.Customer.LastName} - {book.ServiceType.ServiceTypeName}"
            });
            return bo.ToList();
        }

        private async Task<Guid> CreateArrivalToken(Book book)
        {
            book.ArrivalToken = Guid.NewGuid();

            await dbContext.SaveChangesAsync();
            return book.ArrivalToken.GetValueOrDefault();
            //create a tinyURL
        }

        public async Task SetBookAsync(
            DateTime startDate,
            int customerId,
            int startAt,
            List<int> serviceTypeList)
        {
            if (customerId == 0)
                throw new NotFoundException("Customer was not found");

            if (await dbContext.Books.AnyAsync(book =>
                  book.StartDate.Date == startDate.Date
                  && book.StartAt == startAt))
                throw new ConflictException("This is not free day and time");

            if (await dbContext.Books
                .Where(b => b.StartDate.Date == startDate.Date)
                .AnyAsync(book => book.StartAt <= startAt
                        && startAt < (book.StartAt + book.Durtion)))
                throw new ConflictException("This is not free day and time");

            for (int i = 0; i < serviceTypeList.Count; i++)
            {

                var serviceTypeQuery = dbContext.Servicetypes
                    .Where(se => se.ServiceTypeId == serviceTypeList[i]);

                var serviceId = await serviceTypeQuery
                    .Select(st => st.ServiceId)
                    .FirstOrDefaultAsync();


                if (serviceId <= 0)
                    throw new ArgumentNullException("your serviceType Id not exist"); //Exeption Manager

                var duration = await serviceTypeQuery
                    .Select(st => st.Duration)
                    .FirstOrDefaultAsync();

                var book = new DAL.Model.Book()  // B900-B960
                {
                    CustomerId = customerId,
                    StartDate = startDate,
                    StartAt = startAt,
                    ServiceId = serviceId,
                    ServiceTypeId = serviceTypeList[i],
                    Durtion = duration.GetValueOrDefault(),
                    ArrivalToken = Guid.NewGuid()
                };
                dbContext.Books.Add(book);
                await dbContext.SaveChangesAsync();

                if (await settingsService.GetValue(SettingsNames.SEND_SMS_APP) == "1")
                    await SendBookConfirm(book.BookId);

                if (i == 0)
                    startAt += duration.GetValueOrDefault();
            }

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
                throw new NotFoundException();

            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();

        }
        public async Task<List<DTO.Book>> GetCustomerFutureBooksAsync()
        {

            if (userContext.Identity.UserId == 0)
                throw new Exception();
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
        public async Task SetArrival(Guid arrivalToken, int arrivalConfrim)
        {
            if (arrivalToken == Guid.Empty)
                throw new NotFoundException();

            var book = await dbContext.Books
                .Where(b => b.ArrivalToken == arrivalToken)
                .FirstOrDefaultAsync();

            book.ArrivalStatus = arrivalConfrim;
            book.ArrivalToken = null;
            await dbContext.SaveChangesAsync();
        }

        public async Task<DTO.Book> GetBookByArrivalToken(Guid arrivalToken)
        {
            var book = await dbContext.Books
                .Include(c => c.Customer)
                .Include(st => st.ServiceType)
                .Where(b => b.ArrivalToken == arrivalToken)
                .FirstOrDefaultAsync();

            return mapper.Map<DTO.Book>(book);
        }
        public async Task<List<DTO.Book>> SendRemainderBook()
        {
            var RemainderBooks = await dbContext.Books
                .Include(c => c.Customer)
                .Include(st => st.ServiceType)
                .Where(b => b.StartDate.Date == DateTime.Now.AddDays(1).Date)
                .ToListAsync();

            var messageTemplate = await settingsService.GetValue(SettingsNames.SMS_TEMPLATE_REMINDER);
            foreach (var book in RemainderBooks)
            {
                //TODO : [GG] need to add LINK for Arrival confirm
                var parameters = new Dictionary<string, string>();
                parameters.Add("{FirstName}", book.Customer.FirstName);
                parameters.Add("{LastName}", book.Customer.LastName);
                parameters.Add("{Date}", book.StartDate.ToShortDateString());
                parameters.Add("{Time}", book.StartDate.Date.AddMinutes(book.StartAt).ToShortTimeString());
                parameters.Add("{ServiceType}", book.ServiceType.ServiceTypeName);
                parameters.Add("{Link}", "link");
                parameters.Add("\\n", System.Environment.NewLine);

                var message = MessageHelper.ReplacePlaceHolder(messageTemplate, parameters);
                //send sms or not
                var results = clientFactory
                    .GetGlobalSmsSenderClient()
                    .WithUri()
                    .WithSender("Miritush")
                    .ToPhoneNumber(book.Customer.PhoneNumber)
                    .Message(message)
                    .GetAsync();
            }

            return mapper.Map<List<DTO.Book>>(RemainderBooks);
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
            var results = (await clientFactory
                .GetGlobalSmsSenderClient()
                .WithUri()
                .WithSender("Miritush")
                .ToPhoneNumber(newBook.Customer.PhoneNumber)
                .Message(message)
                .GetAsync())
                .AssertResultAsync<DTO.GlobalSmsResult>();

            return true;
        }

    }
}