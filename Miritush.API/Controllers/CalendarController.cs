using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miritush.API.Attributes;
using Miritush.API.Model;
using Miritush.DTO;
using Miritush.DTO.Const;
using Miritush.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly IBookService bookService;

        public CalendarController(
            ICalendarService calendarService,
            IBookService bookService)
        {
            _calendarService = calendarService;
            this.bookService = bookService;
        }

        [HttpGet("closeDays")]
        public async Task<List<CloseDay>> GetCloseDayAndHoliday()
            => await _calendarService.GetCloseDayAndHolidayAsync();

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("closeDays")]
        [ValidateModel]
        public async Task<IActionResult> CreateCloseDay(CreateCloseDayData data)
        {
            await _calendarService.CreateCloseDay(data.Date, data.Notes);
            return StatusCode(StatusCodes.Status201Created);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("book/update")]
        public async Task<IActionResult> UpdateBook(UpdateBookData data)
        {
            await bookService.UpdateBookAsync(
                data.BookId,
                data.StartDate,
                data.StartAt,
                data.CustomerId,
                data.Notes);

            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("book/create")]
        public async Task<IActionResult> CreateBook(CreateBookData data)
        {
            await bookService.SetBookAsync(
                data.StartDate,
                data.CustomerId,
                data.StartAt,
                data.ServiceTypeIds);

            return StatusCode(StatusCodes.Status201Created);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("book/{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            await bookService.DeleteBook(id);

            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("closeDays/{id}")]
        [ValidateModel]
        public async Task<IActionResult> DeleteCloseDay([FromRoute] int id)
        {
            await _calendarService.DeleteCloseDay(id);
            return Ok();
        }
        [HttpGet("freedays")]
        public async Task<ListResult<FreeSlots>> FreeDays([FromQuery] ListFreeDaysData data)

            => await _calendarService.GetFreeDaysAsync(
                data.startDate,
                data.duration.GetValueOrDefault(),
                data.pageNumber,
                data.pageSize);

        [AllowAnonymous]
        [HttpGet("closeDays/updateHolidays")]
        public async Task UpdateHoliday()
         => await _calendarService.UpdateHolidaysAsync();
    }
}
