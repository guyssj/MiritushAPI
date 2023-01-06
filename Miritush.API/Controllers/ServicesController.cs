using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Model;
using Miritush.DTO;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        private readonly ILockHoursService _lockHoursService;

        public ServicesController(IServicesService servicesService, ILockHoursService lockHoursService)
        {
            _lockHoursService = lockHoursService;
            _servicesService = servicesService;
        }
        [HttpGet]
        public async Task<List<DTO.Service>> GetServices()
            => await _servicesService.GetServicesAsync();

        [AllowAnonymous]
        [HttpPost]
        public async Task<DTO.Service> Create(CreateServiceData serviceData)
            => await _servicesService.CreateServiceAsync(serviceData.ServiceName);
    }
}