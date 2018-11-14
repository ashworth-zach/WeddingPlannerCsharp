using System;
using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string logemail { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string logpassword { get; set; }
    }
}
