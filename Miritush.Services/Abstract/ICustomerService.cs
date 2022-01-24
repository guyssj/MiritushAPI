using Miritush.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer> CreateCustomer(string firstName,
                                                  string lastName,
                                                  string phoneNumber,
                                                  string color = "",
                                                  string notes = "");
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> GetCustomerByPhoneNumberAsync(string phoneNumber);
        Task<int> DeleteCustomerAsync(int id);
    }
}