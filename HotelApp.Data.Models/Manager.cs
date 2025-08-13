namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Manager in the system")]
    public class Manager
    {
        [Comment("Manager identifier")]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        [Comment("Manager's user entity")]
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<Booking> ManagedBookings { get; set; }
            = new HashSet<Booking>();
    }
}
