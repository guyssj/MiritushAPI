using Miritush.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ICustomerService
    {
        Task<List<DTO.Customer>> GetCustomersAsync();
        Task<DTO.Customer> CreateCustomer(string firstName,
                                                  string lastName,
                                                  string phoneNumber,
                                                  string color = "",
                                                  string notes = "");
        Task<DTO.Customer> GetCustomerByIdAsync(int id);
        Task<DTO.Customer> GetCustomerByPhoneNumberAsync(string phoneNumber);
        Task<int> DeleteCustomerAsync(int id);
    }
}