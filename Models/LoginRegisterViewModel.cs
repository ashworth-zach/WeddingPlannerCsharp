using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace weddingplanner.Models
{
        public class LoginRegisterViewModel
        {
        public UserRegistration UserReg{get; set;}
        public UserLogin UserLog{get; set;}
        }
}
    // public class LoginRegisterViewModel
    // {
    //     [EmailAddress]

    //     public string LoginEmail { get; set; }
    //     [MinLength(8)]
    //     [DataType(DataType.Password)]
    //     public string LoginPassword { get; set; }
    //     [EmailAddress]

    //     public string RegisterEmail { get; set; }
    //     [MinLength(8)]
    //     [DataType(DataType.Password)]
    //     public string RegisterPassword { get; set; }
    //     [MinLength(3)]

    //     public string RegisterFirstName { get; set; }
    //     [MinLength(3)]

    //     public string RegisterLastName { get; set; }
        
    //     [Compare("RegisterPassword")]
    //     [DataType(DataType.Password)]
    //     public string RegisterConfirm { get; set; }
    // }
