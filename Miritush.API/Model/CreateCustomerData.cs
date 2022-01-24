﻿using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class CreateCustomerData
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        public string Color { get; set; }
        public string Notes { get; set; }

    }
}