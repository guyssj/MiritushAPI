using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class CustomerService: ICustomerService
    {
       public booksDbContext dbContext { get; }

        public CustomerService(booksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            var customers =  await dbContext.Customers.ToListAsync();

            return customers;
        
        }
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await dbContext.Customers.FindAsync(id);
        }
        public async Task<Customer> GetCustomerByPhoneNumberAsync(string phoneNumber)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            return await dbContext.Customers.Where(x => x.PhoneNumber == phoneNumber)
                                            .FirstOrDefaultAsync();
        }
        public async Task<Customer> CreateCustomer(string firstName,
                                                   string lastName,
                                                   string phoneNumber,
                                                   string color = "",
                                                   string notes = "")
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new System.ArgumentException($"'{nameof(firstName)}' cannot be null or whitespace.", nameof(firstName));
           
            if (string.IsNullOrWhiteSpace(lastName))
                throw new System.ArgumentException($"'{nameof(lastName)}' cannot be null or whitespace.", nameof(lastName));


            var customer = await GetCustomerByPhoneNumberAsync(phoneNumber);
            if (customer != null)
                return customer;

            customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Color = color,
                Notes = notes
            };

            dbContext.Customers.Add(customer);
            await dbContext.SaveChangesAsync();

            return customer;

        }

        public async Task<Customer> UpdateCustomerAsync(int id)
        {
            var customer = await dbContext.Customers.FindAsync(id);

            customer.CustomerId = id;
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customerToRemove = await GetCustomerByIdAsync(id);
            dbContext.Customers.Remove(customerToRemove);

            return await dbContext.SaveChangesAsync();

        }

    }
}
