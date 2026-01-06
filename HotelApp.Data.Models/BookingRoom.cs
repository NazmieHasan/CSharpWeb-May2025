namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Mapping table between Booking and Room")]
    public class BookingRoom
    {
        public BookingRoom()
        {
            Id = Guid.NewGuid();
            StatusId = 1; // Awaiting Payment
        }

        [Comment("BookingRoom identifier")]
        public Guid Id { get; set; }

        [Comment("Booking identifier")]
        public Guid BookingId { get; set; }

        public virtual Booking Booking { get; set; } = null!;

        [Comment("Room identifier")]
        public Guid RoomId { get; set; }

        public virtual Room Room { get; set; } = null!;

        [Comment("Status of the room reservation")]
        public int StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;

        public int AdultsCount { get; set; }

        [Comment("Child age is between 4 and 17")]
        public int ChildCount { get; set; }

        [Comment("Baby age is between 0 and 3")]
        public int BabyCount { get; set; }

        [Comment("Shows if booking room is deleted")]
        public bool IsDeleted { get; set; }

        public ICollection<Stay> Stays { get; set; } = new HashSet<Stay>();
    }
}
