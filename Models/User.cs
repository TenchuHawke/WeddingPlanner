using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // ** Change this for a one-to-many style connection
        public List<WeddingGuest> GuestAtWeddings { get; set; }
        public List<Wedding> WeddingsPlanned { get; set; }
        public bool Registered {get; set;}
        public User()
        {
            GuestAtWeddings = new List<WeddingGuest>();
            WeddingsPlanned = new List<Wedding>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}