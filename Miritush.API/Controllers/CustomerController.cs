using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Attributes;
using Miritush.API.Model;
using Miritush.DAL.Model;
using Miritush.Services;
using Miritush.Services.Abstract;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ITransactionService _transactionService;

        public CustomerController(ICustomerService customerService, ITransactionService transactionService)
        {
            _customerService = customerService;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get Customers only admin users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<List<DTO.Customer>> Get()
        {
            return _customerService.GetCustomersAsync();
        }

        /// <summary>
        /// Get customer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<DTO.Customer> GetCustomerById(int id)
        {
            return await _customerService.GetCustomerByIdAsync(id);
        }

        /// <summary>
        /// Get transaction for customer id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet("{id}/transactions")]
        public async Task<List<DTO.Transaction>> GetTransactionsByCustomerId(int id, CancellationToken cancelToken = default)
        {
            return await _transactionService.GetTransactionsByCustomerIdAsync(id, cancelToken);
        }

        /// <summary>
        ///  Get Future books by customer id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet("{id}/futurebooks")]
        public async Task<List<DTO.Book>> GetFutureBooks(int id, CancellationToken cancelToken = default)
        {
            return await _customerService.GetFutureBooksByCustomerId(id, cancelToken);
        }

        /// <summary>
        ///  Get Future books by customer id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet("{id}/timeline")]
        public async Task<List<DTO.CustomerTimeline>> GetTimelineAsync(int id, CancellationToken cancelToken = default)
        {
            return await _customerService.GetCustomerTimelinesAsync(id, cancelToken);
        }


        /// <summary>
        /// Get Customer by phone number admin only
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet("phoneNumber")]
        public Task<DTO.Customer> GetCustomerByPhoneNumber([FromQuery] string phoneNumber)
        {
            return _customerService.GetCustomerByPhoneNumberAsync(phoneNumber);
        }

        // POST api/<CustomerController>
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public Task<DTO.Customer> CreateCutomer([FromBody] CustomerData customerData)
        {
            return _customerService.CreateCustomer(
                 customerData.FirstName,
                 customerData.LastName,
                 customerData.PhoneNumber,
                 customerData.Color,
                 customerData.Notes);
        }


        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<DTO.Customer> Update(int id, [FromBody] CustomerData data)
        {
            return await _customerService.UpdateCustomerAsync(
                 data.Id,
                 data.FirstName,
                 data.LastName,
                 data.PhoneNumber,
                 data.Color,
                 data.Notes);
        }
    }
}
