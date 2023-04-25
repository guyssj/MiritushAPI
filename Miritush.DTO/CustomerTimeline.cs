using System;
using Miritush.DTO.Enums;

namespace Miritush.DTO
{
    public class CustomerTimeline
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public TimelineType Type { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public string Notes { get; set; }
    }
}