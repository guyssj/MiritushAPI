using System.Collections.Generic;
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
    }
}
