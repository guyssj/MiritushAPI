using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Model;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ServiceTypesController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;

        public ServiceTypesController(IServiceTypeService serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<List<DTO.ServiceType>> ListServiceTypes()
            => await _serviceTypeService.List();
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<DTO.ServiceType> GetById(int id)

            => await _serviceTypeService.Get(id);
        [AllowAnonymous]
        [HttpGet("service/{serviceId}")]
        public async Task<List<DTO.ServiceType>> GetByService(int serviceId)
             => await _serviceTypeService.GetByService(serviceId);

        [AllowAnonymous]
        [HttpPost()]
        public async Task CreateServiceType(CreateServiceTypeData data)
            => await _serviceTypeService.CreateServiceType(
               data.Name,
               data.ServiceId,
               data.Duration,
               data.Price,
               data.Description);
    }
}