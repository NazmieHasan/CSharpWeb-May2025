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
                .Property(b => b.UserId)
                .IsRequired();

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
                .HasQueryFilter(b => b.IsDeleted == false);

            entity
                .HasData(this.GenerateSeedBooking());
        }

        private List<Booking> GenerateSeedBooking()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            List<Booking> seedBooking = new List<Booking>()
            {
                new Booking
                {
                    //Id = Guid.NewGuid(),
                    Id = Guid.Parse("7da78485-b70d-4770-84f8-152ed4d9ccee"),
                    CreatedOn = new DateTime(2025, 8, 30, 13, 13, 54, DateTimeKind.Utc),
                    DateArrival = DateOnly.Parse("2025-08-30"),
                    DateDeparture = DateOnly.Parse("2025-08-31"),
                    AdultsCount = 2,
                    ChildCount = 0,
                    BabyCount = 0,
                    IsDeleted = false,
                    UserId = "1b00f3f5-43ed-41f6-bdf2-ad5266370038",
                    RoomId = Guid.Parse("AE50A5AB-9642-466F-B528-3CC61071BB4C")
                },
                new Booking
                {
                    //Id = Guid.NewGuid(),
                    Id = Guid.Parse("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                    CreatedOn = new DateTime(2025, 8, 30, 13, 14, 54, DateTimeKind.Utc),
                    DateArrival = DateOnly.Parse("2025-08-30"),
                    DateDeparture = DateOnly.Parse("2025-08-31"),
                    AdultsCount = 2,
                    ChildCount = 0,
                    BabyCount = 0,
                    IsDeleted = false,
                    UserId = "1b00f3f5-43ed-41f6-bdf2-ad5266370038",
                    RoomId = Guid.Parse("68FB84B9-EF2A-402F-B4FC-595006F5C275")
                },
                new Booking
                {
                    //Id = Guid.NewGuid(),
                    Id = Guid.Parse("eb003919-0478-4b33-a168-170c78a8750b"),
                    CreatedOn = new DateTime(2025, 8, 30, 13, 15, 54, DateTimeKind.Utc),
                    DateArrival = DateOnly.Parse("2025-08-30"),
                    DateDeparture = DateOnly.Parse("2025-08-31"),
                    AdultsCount = 1,
                    ChildCount = 0,
                    BabyCount = 0,
                    IsDeleted = false,
                    UserId = "9c74337f-64e2-40ad-8bde-09b2631d17cb",
                    RoomId = Guid.Parse("777634E2-3BB6-4748-8E91-7A10B70C78AC")
                }
            };

            return seedBooking;
        }
    }
}
