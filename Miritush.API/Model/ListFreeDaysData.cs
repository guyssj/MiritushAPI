using System;
using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class ListFreeDaysData : ListPagingData
    {
        [Required]
        public DateTime startDate { get; set; }
        public int? duration { get; set; }

    }
}
