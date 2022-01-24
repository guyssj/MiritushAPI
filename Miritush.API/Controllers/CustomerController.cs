using Microsoft.AspNetCore.Mvc;
using Miritush.API.Model;
using Miritush.DAL.Model;
using Miritush.Services;
using Miritush.Services.Abstract;
using System.Collections.Generic;
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

		public CustomerController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

        // GET: api/<CustomerController>
        [HttpGet]
        public Task<List<Customer>> Get()
        {
            return _customerService.GetCustomersAsync();
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public Task<Customer> GetCustomerById(int id)
        {
            return _customerService.GetCustomerByIdAsync(id);
        }

        // GET api/<CustomerController>/0504277550
        [HttpGet("{phoneNumber}")]
        public Task<Customer> GetCustomerByPhoneNumber(string phoneNumber)
        {
            return _customerService.GetCustomerByPhoneNumberAsync(phoneNumber);
        }

        // POST api/<CustomerController>
        [HttpPost]
        public Task<Customer> CreateCutomer([FromBody] CreateCustomerData customerData)
        {
           return _customerService.CreateCustomer(
                customerData.FirstName,
                customerData.LastName,
                customerData.PhoneNumber,
                customerData.Color,
                customerData.Notes);
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
