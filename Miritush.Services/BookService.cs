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
        private readonly ICustomerTimelineService customerTimelineService;

        public BookService(
            booksDbContext dbContext,
            IUserContextService userContext,
            IMapper mapper,
            ISettingsService settingsService,
            IHttpClientFactory clientFactory,
            ICustomerTimelineService customerTimelineService)
        {
            this.dbContext = dbContext;
            this.userContext = userContext;
            this.mapper = mapper;
            this.settingsService = settingsService;
            this.clientFactory = clientFactory;
            this.customerTimelineService = customerTimelineService;
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
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            return await dbContext.Books
                .AsNoTracking()
                .CountAsync(book => book.StartDate >= today && book.StartDate < tomorrow);
        }

        public async Task<int> GetMonthBooksCountAsync()
        {
            var date = DateTime.UtcNow;
            var firstDayInMonth = new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var firstDayNextMonth = firstDayInMonth.AddMonths(1);

            return await dbContext.Books
                .AsNoTracking()
                .CountAsync(book => book.StartDate >= firstDayInMonth && book.StartDate < firstDayNextMonth);
        }

        public async Task<List<Book>> GetNextBooks()
        {
            var books = await dbContext.Books
                .AsNoTracking()
                .Where(book => book.StartDate.Date >= DateTime.UtcNow.Date)
                .Take(5)
                .OrderBy(b => b.StartAt)
                .ToListAsync();

            return books;

        }

        public async Task<List<DTO.CalendarEvent<DTO.Book>>> GetBooksForCalendar()
        {
            var books = await dbContext.Books
                .AsNoTracking()
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

        public async Task<List<DTO.CalendarEventApp<DTO.Book>>> GetBooksForCalendarApp(DateTime? startDate)
        {
            if (startDate == null)
                startDate = DateTime.UtcNow.AddDays(-3);
            var books = await dbContext.Books
                .Where(x => x.StartDate > startDate)
                .AsNoTracking()
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

            var serviceTypes = await dbContext.Servicetypes
                .Where(se => serviceTypeList.Contains(se.ServiceTypeId))
                .ToListAsync();

            var sendSmsSetting = await settingsService.GetValue(SettingsNames.SEND_SMS_APP);
            var shouldSendSms = sendSmsSetting == "1";

            for (int i = 0; i < serviceTypeList.Count; i++)
            {
                var serviceTypeId = serviceTypeList[i];
                var serviceType = serviceTypes
                    .FirstOrDefault(se => se.ServiceTypeId == serviceTypeId);

                if (serviceType == null || serviceType.ServiceId <= 0)
                    throw new ArgumentNullException("your serviceType Id not exist"); //Exeption Manager

                var duration = serviceType.Duration.GetValueOrDefault();

                var book = new DAL.Model.Book()  // B900-B960
                {
                    CustomerId = customerId,
                    StartDate = startDate,
                    StartAt = startAt,
                    ServiceId = serviceType.ServiceId,
                    ServiceTypeId = serviceTypeId,
                    Durtion = duration,
                    ArrivalToken = Guid.NewGuid()
                };
                dbContext.Books.Add(book);
                var timeLineDesc = serviceType.ServiceTypeName;

                //save a timeline table
                await customerTimelineService
                    .SaveTimeLine(customerId, DTO.Enums.TimelineType.Book, timeLineDesc)
                    .ConfigureAwait(false);

                if (await settingsService.GetValue(SettingsNames.SEND_SMS_APP) == "1")
                    //await SendBookConfirm(book.BookId);

                    if (i == 0)
                        startAt += duration;
                if (shouldSendSms)
                    await SendBookConfirm(book.BookId);

                if (i == 0)
                    startAt += duration;


            }
            await SendSocketNotification();

            await dbContext.SaveChangesAsync();

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
            var today = DateTime.UtcNow.Date;

            var books = await dbContext.Books
                .AsNoTracking()
                .Where(book => book.CustomerId == customerId
                        && book.StartDate.Date >= today)
                .ToListAsync();

            return mapper.Map<List<DTO.Book>>(books);
        }

        public async Task<List<DTO.Book>> GetBooksByPhoneNumberAsync(string phoneNumber)
        {
            var customerId = await dbContext.Customers
                .AsNoTracking()
                .Where(cus => cus.PhoneNumber == phoneNumber)
                .Select(x => x.CustomerId)
                .FirstOrDefaultAsync();

            if (customerId == 0)
                return new List<DTO.Book>();

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
                .AsNoTracking()
                .Include(c => c.Customer)
                .Include(st => st.ServiceType)
                .Where(b => b.ArrivalToken == arrivalToken)
                .FirstOrDefaultAsync();

            return mapper.Map<DTO.Book>(book);
        }
        public async Task<List<DTO.Book>> SendRemainderBook()
        {
            var targetDate = DateTime.UtcNow.AddDays(1).Date;

            var remainderBooks = await dbContext.Books
                .AsNoTracking()
                .Include(c => c.Customer)
                .Include(st => st.ServiceType)
                .Where(b => b.StartDate.Date == targetDate)
                .ToListAsync();

            if (!remainderBooks.Any())
                return new List<DTO.Book>();

            var messageTemplate = await settingsService.GetValue(SettingsNames.SMS_TEMPLATE_REMINDER);
            var smsClient = clientFactory.GetGlobalSmsSenderClient();
            var sendTasks = new List<Task>();

            foreach (var book in remainderBooks)
            {
                //TODO : [GG] need to add LINK for Arrival confirm
                var parameters = new Dictionary<string, string>
                {
                    ["{FirstName}"] = book.Customer.FirstName,
                    ["{LastName}"] = book.Customer.LastName,
                    ["{Date}"] = book.StartDate.ToShortDateString(),
                    ["{Time}"] = book.StartDate.Date.AddMinutes(book.StartAt).ToShortTimeString(),
                    ["{ServiceType}"] = book.ServiceType.ServiceTypeName,
                    ["{Link}"] = "link",
                    ["\\n"] = Environment.NewLine
                };

                var message = MessageHelper.ReplacePlaceHolder(messageTemplate, parameters);

                var sendTask = smsClient
                    .WithUri()
                    .WithSender("Miritush")
                    .ToPhoneNumber(book.Customer.PhoneNumber)
                    .Message(message)
                    .GetAsync();

                sendTasks.Add(sendTask);
            }

            await Task.WhenAll(sendTasks);

            return mapper.Map<List<DTO.Book>>(remainderBooks);
        }


        private async Task SendSocketNotification()
        {
            var payload = new
            {
                GroupName = "BOOK_STATE",
                ObjectId = 1
            };
            _ = await clientFactory
                .GetSocketAPISenderClient()
                .WithUri()
                .WithMethod("notifications")
                .PostAsync(payload);
        }
        private async Task<bool> SendBookConfirm(int bookId)
        {
            var newBook = await dbContext.Books
                .AsNoTracking()
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