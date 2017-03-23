using System;

using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models
{
    public class WeddingValidation : Wedding
    {
        [Required]
        [MinLength(2)]
        public new string EventName { get; set; }
        [Required]
        [InTheFuture]
        public new DateTime EventDate { get; set; }
        [Required]
        [MinLength(2)]
        public new string SideA { get; set; }
        [Required]
        [MinLength(2)]
        public new string SideB { get; set; }
        public new int OwnerId {get; set;}
        public Wedding ToWedding()
        {
            Wedding NewWedding = new Wedding
            {
                EventName = this.EventName,
                    EventDate = this.EventDate,
                    SideA = this.SideA,
                    SideATitle = this.SideATitle,
                    SideB = this.SideB,
                    SideBTitle = this.SideBTitle,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OwnerId = this.OwnerId
            };
            return NewWedding;
        }
    }
}