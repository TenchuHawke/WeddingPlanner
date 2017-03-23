using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models
{
    public class Wedding : BaseEntity
    {
        [Key]
        public int WeddingId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        [ForeignKey("UserId")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string SideA { get; set; }
        public string SideATitle { get; set; }
        public string SideB { get; set; }
        public string SideBTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKeyAttribute("Wedding")]
        public List<WeddingGuest> GuestsAttending { get; set; }
        public Wedding()
        {
            GuestsAttending = new List<WeddingGuest>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public int RSVPCount(){
            int count =0;
            foreach(WeddingGuest guest in GuestsAttending){
                if(!guest.Pending){
                    count++;
                }
            }
                return count;
        }
    }
}