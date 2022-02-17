using System;
using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class ListFreeDaysData
    {
        [Required]
        public DateTime startDate { get; set; }
        public int? duration { get; set; }

        [Range(1, int.MaxValue)]
        public int pageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int pageSize { get; set; } = 20;

    }
}
