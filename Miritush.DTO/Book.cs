using System;
using Miritush.DTO.Enums;

namespace Miritush.DTO
{
    public class Book
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int StartAt { get; set; }
        public int Duration { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public string Notes { get; set; }
        public ArrivalConfirm Arrival { get; set; }
    }
}