namespace HotelApp.Data.Configuration
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ApplicationUserBookingConfiguration : IEntityTypeConfiguration<ApplicationUserBooking>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserBooking> entity)
        {
            // Define composite Primary Key of the Mapping Entity
            entity
                .HasKey(aub => new { aub.ApplicationUserId, aub.BookingId });

            // Define required constraint for the ApplicationUserId, as it is type string
            entity
                .Property(aub => aub.ApplicationUserId)
                .IsRequired();

            // Define default value for soft-delete functionality
            entity
                .Property(aub => aub.IsDeleted)
                .HasDefaultValue(false);

            // Configure relation between ApplicationUserBooking and IdentityUser
            // The IdentityUser does not contain navigation property, as it is built-in type from the ASP.NET Core Identity
            entity
                .HasOne(aub => aub.ApplicationUser)
                .WithMany() // We do not have navigation property from the IdentityUser side
                .HasForeignKey(aub => aub.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relation between ApplicationUserBooking and Booking
            entity
                .HasOne(aub => aub.Booking)
                .WithMany(b => b.UserBookings)
                .HasForeignKey(aub => aub.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define query filter to hide the ApplicationUserMovie entries referring deleted Movie
            // Solves the problem with relations during delete
            entity
                .HasQueryFilter(aub => aub.Booking.IsDeleted == false);

            // Define query filter to hide the deleted entries in the user Watchlist
            entity
                .HasQueryFilter(aub => aub.IsDeleted == false);
        }
    }
}
