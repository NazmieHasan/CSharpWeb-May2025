namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> entity)
        {
            entity
                .HasKey(b => b.Id);

            entity
                .Property(b => b.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            entity
                .Property(b => b.StatusId)
                .HasDefaultValue(1);

            entity.ToTable(tb =>
            {
                // Ensure DateArrival is not in the past (UTC)
                tb.HasCheckConstraint(
                    "CK_Booking_DateArrival_NotPast",
                    "[DateArrival] >= CONVERT(date, GETUTCDATE())"
                );

                // Ensure DateDeparture is at least one day after DateArrival
                tb.HasCheckConstraint(
                    "CK_Booking_DepartureAfterArrival",
                    "[DateDeparture] > [DateArrival]"
                );
            });

            entity
                .Property(b => b.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(r => r.Room)
                .WithMany(b => b.Bookings)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(s => s.Status)
                .WithMany(b => b.Bookings)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(c => c.Manager)
                .WithMany(m => m.ManagedBookings)
                .HasForeignKey(c => c.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity
               .Property(b => b.StatusId)
               .HasDefaultValue(1);

            entity
                .HasQueryFilter(b => b.IsDeleted == false);

        }
    }
}
