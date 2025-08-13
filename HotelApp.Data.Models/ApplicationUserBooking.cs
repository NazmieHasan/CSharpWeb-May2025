namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("User Booking entry in the system.")]
    public class ApplicationUserBooking
    {
        [Comment("Foreign key to the referenced AspNetUser. Part of the entity composite PK.")]
        public string ApplicationUserId { get; set; } = null!;

        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [Comment("Foreign key to the referenced Booking. Part of the entity composite PK.")]
        public Guid BookingId { get; set; }

        public virtual Booking Booking { get; set; } = null!;

        [Comment("Shows if ApplicationUserBooking entry is deleted")]
        public bool IsDeleted { get; set; }
    }
}
