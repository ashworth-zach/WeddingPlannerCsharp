using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace weddingplanner.Models
{
    public class UserRegistration
    {

        [Required]
        [MinLength(3)]
        public string firstname { get; set; }

        [Required]
        [MinLength(3)]
        public string lastname { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        public string password { get; set; }
                [NotMapped]
        [Compare("password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}