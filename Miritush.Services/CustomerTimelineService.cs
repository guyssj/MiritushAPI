using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class CustomerTimelineService : ICustomerTimelineService
    {
        private readonly booksDbContext _booksDbContext;
        private readonly IMapper _mapper;

        public CustomerTimelineService(
            booksDbContext booksDbContext,
            IMapper mapper)
        {
            _booksDbContext = booksDbContext;
            _mapper = mapper;
        }

        public async Task SaveTimeLine(
            int customerId,
            DTO.Enums.TimelineType type,
            string description = null,
            string notes = null)
        {
            var timeline = new CustomerTimeline
            {
                CustomerId = customerId,
                Description = description,
                Notes = notes,
                Type = (int)type
            };

            _booksDbContext.CustomerTimelines.Add(timeline);
            await _booksDbContext.SaveChangesAsync();
        }
    }
}