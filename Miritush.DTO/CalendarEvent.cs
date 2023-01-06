using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class CalendarEvent<T>
    {
        public string Title { get; set; }
        public bool AllDay { get; set; } = false;
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public T Meta { get; set; }
        public ServiceType ServiceType { get; set; }
        public Customer Customer { get; set; }
    }
    public class CalendarEventApp<T>
    {
        public string Title { get; set; }
        public bool AllDay { get; set; } = false;
        public DateTime End { get; set; }
        public DateTime start { get; set; }
        public T Meta { get; set; }
        public ServiceType ServiceType { get; set; }
        public Customer Customer { get; set; }
    }
}