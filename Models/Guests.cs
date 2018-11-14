using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace weddingplanner.Models
{   
    [Table("guests", Schema = "weddingdb")]
    public class Guests
    {
        // auto-implemented properties need to match columns in your table
        [Key]
        public int GuestId { get; set; }
        public int UserId {get;set;}
        public User user {get;set;}
        public int WeddingId{get;set;}
        public Wedding wedding {get;set;}

    }
}