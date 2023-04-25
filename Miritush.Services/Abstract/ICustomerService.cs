using Miritush.DAL.Model;
using System.Collections.Generic;
using System.Threading;
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
        Task<DTO.Customer> UpdateCustomerAsync(
            int id,
            string firstName,
            string lastName,
            string phoneNumber,
            string color,
            string notes,
            int otp = 0,
            bool active = false);

        /// <summary>
        /// Get Future books by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task<List<DTO.Book>> GetFutureBooksByCustomerId(
        int customerId,
        CancellationToken cancelToken = default);

        /// <summary>
        /// get customer timeline by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task<List<DTO.CustomerTimeline>> GetCustomerTimelinesAsync(
            int customerId,
            CancellationToken cancelToken);
    }
}