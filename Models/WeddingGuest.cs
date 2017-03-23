using System;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models
{
    public class WeddingGuest : BaseEntity
    {
        [Key]
        public int WeddingGuestId { get; set; }
        [ForeignKeyAttribute("WeddingId")]
        public int EventId { get; set; }
        public Wedding Event { get; set; }
        [ForeignKey("UserId")]
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public bool GuestOfSideA { get; set; }
        public bool Pending { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public WeddingGuest(Wedding NewWedding, User NewUser, bool side)
        {
            EventId = NewWedding.WeddingId;
            Event = NewWedding;
            GuestId = NewUser.UserId;
            Guest = NewUser;
            GuestOfSideA = side;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Pending = true;
        }
        public WeddingGuest()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Pending = true;
        }
    }
}