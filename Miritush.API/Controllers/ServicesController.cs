using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        public async Task<List<DTO.Service>> GetServices()
            => await _servicesService.GetServicesAsync();
        [HttpPost]
        public async Task<DTO.Service> Create(CreateServiceData serviceData)
            => await _servicesService.CreateServiceAsync(serviceData.ServiceName);
    }
}