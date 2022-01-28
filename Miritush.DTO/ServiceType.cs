using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServiceId { get; set; }
        public int? Duration { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; } = "";
    }
}