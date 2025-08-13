namespace HotelApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public virtual Manager? Manager { get; set; }

        public virtual ICollection<ApplicationUserBooking> BookingList { get; set; }
            = new HashSet<ApplicationUserBooking>();

        public virtual ICollection<Booking> Bookings { get; set; }
            = new HashSet<Booking>();
    }
}
