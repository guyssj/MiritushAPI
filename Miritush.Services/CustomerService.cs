using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.DTO;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly booksDbContext dbContext;
        private readonly IMapper mapper;

        public CustomerService(
            booksDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<List<DTO.Customer>> GetCustomersAsync()
        {
            var customers = await dbContext.Customers.ToListAsync();
            return mapper.Map<List<DTO.Customer>>(customers);
        }

        public async Task<DTO.Customer> GetCustomerByIdAsync(int id)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            return mapper.Map<DTO.Customer>(customer);
        }
        public async Task<DTO.Customer> GetCustomerByPhoneNumberAsync(string phoneNumber)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var customer = await dbContext.Customers.Where(x => x.PhoneNumber == phoneNumber)
                                            .FirstOrDefaultAsync();

            return mapper.Map<DTO.Customer>(customer);

        }

        public async Task<List<DTO.CustomerTimeline>> GetCustomerTimelinesAsync(
            int customerId,
            CancellationToken cancelToken)
        {
            var customerTimeline = await dbContext.CustomerTimelines
                .Where(ct => ct.CustomerId == customerId)
                .ToListAsync(cancelToken);

            return mapper.Map<List<DTO.CustomerTimeline>>(customerTimeline);

        }

        public async Task<List<DTO.Book>> GetFutureBooksByCustomerId(
            int customerId,
            CancellationToken cancelToken = default)
        {
            if (customerId <= 0)
                throw new ArgumentNullException(nameof(customerId));

            var futureBooks = await dbContext.Books
                .Where(b => b.CustomerId == customerId && b.StartDate.Date.AddMinutes(b.StartAt) >= DateTime.UtcNow.Date)
                .ToListAsync(cancelToken);

            return mapper.Map<List<DTO.Book>>(futureBooks);

        }
        public async Task<DTO.Customer> CreateCustomer(string firstName,
                                                   string lastName,
                                                   string phoneNumber,
                                                   string color = "",
                                                   string notes = "")
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new System.ArgumentException($"'{nameof(firstName)}' cannot be null or whitespace.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new System.ArgumentException($"'{nameof(lastName)}' cannot be null or whitespace.", nameof(lastName));


            var customer = await dbContext.Customers.Where(x => x.PhoneNumber == phoneNumber)
                                            .FirstOrDefaultAsync();
            if (customer != null)
                return mapper.Map<DTO.Customer>(customer);


            customer = new DAL.Model.Customer()
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Color = color,
                Notes = notes
            };

            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();

            return mapper.Map<DTO.Customer>(customer);

        }

        public async Task<DTO.Customer> UpdateCustomerAsync(
            int id,
            string firstName,
            string lastName,
            string phoneNumber,
            string color,
            string notes,
            int otp = 0,
            bool active = false)
        {
            var customer = await dbContext.Customers.FindAsync(id);
            var bypeActive = active ? 0 : 1;

            customer.CustomerId = id;
            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.PhoneNumber = phoneNumber;
            customer.Color = color;
            customer.Notes = notes;
            customer.Otp = otp;
            customer.Active = (byte)bypeActive;

            await dbContext.SaveChangesAsync();

            return mapper.Map<DTO.Customer>(customer);
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customerToRemove = await dbContext.Customers.FindAsync(id);
            dbContext.Customers.Remove(customerToRemove);

            return await dbContext.SaveChangesAsync();

        }

        // Search customers by a single term across all fields with pagination and async
        public async Task<ListResult<DTO.Customer>> SearchCustomersAsync(
            string searchTerm = null,
            int page = 1,
            int pageSize = 10,
            string sortBy = "FirstName",
            bool ascending = true)
        {
            var query = dbContext.Customers.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                    c.FirstName.ToLower().Contains(lowerSearchTerm) ||
                    c.LastName.ToLower().Contains(lowerSearchTerm) ||
                    c.PhoneNumber.Contains(searchTerm));
            }

            // Filter only active customers
            query = query.Where(c => c.Active == 1);

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortBy.ToLower() switch
            {
                "lastname" => ascending ? query.OrderBy(c => c.LastName) : query.OrderByDescending(c => c.LastName),
                "phonenumber" => ascending ? query.OrderBy(c => c.PhoneNumber) : query.OrderByDescending(c => c.PhoneNumber),
                "customerid" => ascending ? query.OrderBy(c => c.CustomerId) : query.OrderByDescending(c => c.CustomerId),
                _ => ascending ? query.OrderBy(c => c.FirstName) : query.OrderByDescending(c => c.FirstName)
            };

            // Apply pagination
            var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<DTO.Customer>(mapper.ConfigurationProvider)
                .ToListAsync();
            return new ListResult<DTO.Customer>(page, pageSize)
            {
                Data = customers,
                TotalRecord = totalCount,
            };
        }


    }
}
