using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Model;
using Miritush.DTO;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LockHoursController : ControllerBase
    {
        private readonly ILockHoursService _lockHoursService;

        public LockHoursController(ILockHoursService lockHoursService)
        {
            _lockHoursService = lockHoursService;

        }

        [HttpGet]
        public async Task<List<CalendarEvent<LockHour>>> GetLock()
             => await _lockHoursService.List();

        [HttpPost]
        public async Task<IActionResult> Create(CreateLockHoursData data)
        {
            await _lockHoursService.CreateLockHour(
             data.StartDate,
             data.StartAt,
             data.EndAt,
             data.Notes);

            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Create(int id)
        {
            await _lockHoursService.DeleteLockHour(id);

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}