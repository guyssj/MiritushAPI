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

        public async Task<DTO.Customer> UpdateCustomerAsync(int id)
        {
            var customer = await dbContext.Customers.FindAsync(id);

            customer.CustomerId = id;

            return mapper.Map<DTO.Customer>(customer);
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customerToRemove = await dbContext.Customers.FindAsync(id);
            dbContext.Customers.Remove(customerToRemove);

            return await dbContext.SaveChangesAsync();

        }

    }
}
