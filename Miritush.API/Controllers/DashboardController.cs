using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Miritush.Services.Abstract;

namespace Miritush.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IMapper mapper;

        public DashboardController(IBookService bookService, IMapper mapper)
        {
            this.bookService = bookService;
            this.mapper = mapper;
        }

        [HttpGet("count/todaybooks")]
        public async Task<int> GetCountTodayBook()
        {
            return await bookService.GetTodayBookCountAsync();
        }
        [HttpGet("Books/Next")]
        public async Task<List<DTO.Book>> GetNextBooks()
        {
            var books = await bookService.GetNextBooks();
            return mapper.Map<List<DTO.Book>>(books);
        }
        [HttpGet("Books/Remainders")]
        public async Task<List<DTO.Book>> SendRemainderBook()
        {
            var books = await bookService.SendRemainderBook();
            return mapper.Map<List<DTO.Book>>(books);
        }
    }
}