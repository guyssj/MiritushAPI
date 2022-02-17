using System;
using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class CreateCloseDayData
    {
        [Required]
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
