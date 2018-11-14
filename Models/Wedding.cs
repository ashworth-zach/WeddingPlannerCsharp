using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace weddingplanner.Models
{
    public class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            if (dt >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
    [Table("wedding", Schema = "weddingdb")]
    public class Wedding
    {
        // auto-implemented properties need to match columns in your table
        [Key]
        public int WeddingId { get; set; }
        public int UserId {get;set;}
        [Required]
        [MinLength(3)]
        public string groom { get; set; }
        [Required]
        [MinLength(3)]
        public string bride { get; set; }
        [Required]
        [CurrentDate]
        public DateTime date { get; set; }
        [Required]
        [MinLength(5)]
        public string address{get;set;}
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [NotMapped]
        public int total{get;set;}
        public List<Guests> guests{get;set;}

        public Wedding()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
            guests = new List<Guests>();
        }

    }
}