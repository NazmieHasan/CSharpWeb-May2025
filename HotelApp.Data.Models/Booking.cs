namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations.Schema;

    [Comment("Booking in the system")]
    public class Booking
    {
        public Booking()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            StatusId = 1; // Awaiting Payment
        } 

        [Comment("Booking identifier")]
        public Guid Id { get; set; } 

        public DateTime CreatedOn { get; set; }
        
        public DateOnly DateArrival { get; set; }

        public DateOnly DateDeparture { get; set; }

        public int AdultsCount { get; set; }

        [Comment("Child age is between 4 and 17")]
        public int ChildCount { get; set; }

        [Comment("Baby age is between 0 and 3")]
        public int BabyCount { get; set; }

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if booking is deleted")]
        public bool IsDeleted { get; set; }

        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        [Comment("Booking's manager")]
        public Guid? ManagerId { get; set; }

        public virtual Manager? Manager { get; set; }

        public Guid RoomId { get; set; }

        public virtual Room Room { get; set; } = null!;

        public int StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;

        public ICollection<Stay> Stays { get; set; } = new HashSet<Stay>();

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        [NotMapped]
        public int DaysCount => (DateDeparture.ToDateTime(TimeOnly.MinValue) - DateArrival.ToDateTime(TimeOnly.MinValue)).Days;

        [NotMapped]
        public decimal TotalAmount => Room?.Category?.Price * DaysCount ?? 0;
    }
}
