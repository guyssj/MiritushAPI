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

        /// <summary>
        /// Asynchronously searches for customers based on a search term, with support for pagination and sorting.
        /// </summary>
        /// <param name="searchTerm">The term to search for in customer data (e.g., name, email). If null or empty, returns all customers.</param>
        /// <param name="page">The page number of the results to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of customers to return per page. Defaults to 10.</param>
        /// <param name="sortBy">The field name to sort the results by (e.g., "FirstName", "LastName"). Defaults to "FirstName".</param>
        /// <param name="ascending">Whether to sort the results in ascending order. If false, results are sorted in descending order. Defaults to true.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a <see cref="DTO.ListResult{DTO.Customer}"/> object with a paginated list of customers that match the search criteria.
        /// </returns>
        Task<DTO.ListResult<DTO.Customer>> SearchCustomersAsync(
            string searchTerm = null,
            int page = 1,
            int pageSize = 10,
            string sortBy = "FirstName",
            bool ascending = true);
    }
}