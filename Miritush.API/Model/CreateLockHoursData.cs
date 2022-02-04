using System;
using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class CreateLockHoursData
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int StartAt { get; set; }
        [Required]
        public int EndAt { get; set; }
        public string Notes { get; set; }
    }
}