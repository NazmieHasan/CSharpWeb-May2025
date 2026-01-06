namespace HotelApp.Data
{
    using System.Reflection;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;
    
    public class HotelAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public HotelAppDbContext(DbContextOptions<HotelAppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;

        public virtual DbSet<Room> Rooms { get; set; } = null!;

        public virtual DbSet<Booking> Bookings { get; set; } = null!;

        public virtual DbSet<Manager> Managers { get; set; } = null!;

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        public virtual DbSet<Payment> Payments { get; set; } = null!;

        public virtual DbSet<Guest> Guests { get; set; } = null!;

        public virtual DbSet<Stay> Stays { get; set; } = null!;

        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;

        public virtual DbSet<Status> Statuses { get; set; } = null!;

        public virtual DbSet<BookingRoom> BookingRooms { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
