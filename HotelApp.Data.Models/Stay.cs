namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    public class Stay
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }

        public DateTime? CheckoutOn { get; set; }

        public Guid BookingId { get; set; }

        public Booking Booking { get; set; } = null!;

        public Guid GuestId { get; set; }

        public Guest Guest { get; set; } = null!;

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if stay is deleted")]
        public bool IsDeleted { get; set; }
    }
}
