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

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if booking is deleted")]
        public bool IsDeleted { get; set; }

        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        [Comment("Owner name of the booking")]
        public string Owner { get; set; } = null!;

        [Comment("Indicates whether the booking is for another person")]
        public bool IsForAnotherPerson { get; set; }

        [Comment("Booking's manager")]
        public Guid? ManagerId { get; set; }

        public virtual Manager? Manager { get; set; }

        public int StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        [NotMapped]
        public int DaysCount => (DateDeparture.ToDateTime(TimeOnly.MinValue) - DateArrival.ToDateTime(TimeOnly.MinValue)).Days;

        [NotMapped]
        public decimal TotalAmount => BookingRooms.Sum(br => br.Room?.Category?.Price * DaysCount ?? 0);

        [NotMapped]
        public int AdultsCount => BookingRooms.Sum(br => br.AdultsCount);

        [Comment("Child age is between 4 and 17")]
        [NotMapped]
        public int ChildCount => BookingRooms.Sum(br => br.ChildCount);

        [Comment("Baby age is between 0 and 3")]
        [NotMapped]
        public int BabyCount => BookingRooms.Sum(br => br.BabyCount);

        public ICollection<BookingRoom> BookingRooms { get; set; }
            = new HashSet<BookingRoom>();
    }
}
