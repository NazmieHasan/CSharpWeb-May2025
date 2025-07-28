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
                .WithMany()
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
                    CreatedOn = DateTime.UtcNow,
                    DateArrival = today.AddDays(1),
                    DateDeparture = today.AddDays(3),
                    AdultsCount = 2,
                    ChildCount = 0,
                    BabyCount = 0,
                    IsDeleted = false,
                    UserId = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    RoomId = Guid.Parse("AE50A5AB-9642-466F-B528-3CC61071BB4C")
                },
                new Booking
                {
                    //Id = Guid.NewGuid(),
                    Id = Guid.Parse("2a523913-dd8e-44d1-a95e-d343ab4d4080"),
                    CreatedOn = DateTime.UtcNow,
                    DateArrival = today.AddDays(2),
                    DateDeparture = today.AddDays(5),
                    AdultsCount = 2,
                    ChildCount = 0,
                    BabyCount = 0,
                    IsDeleted = false,
                    UserId = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    RoomId = Guid.Parse("68FB84B9-EF2A-402F-B4FC-595006F5C275")
                },
                new Booking
                {
                    //Id = Guid.NewGuid(),
                    Id = Guid.Parse("eb003919-0478-4b33-a168-170c78a8750b"),
                    CreatedOn = DateTime.UtcNow,
                    DateArrival = today.AddDays(3),
                    DateDeparture = today.AddDays(4),
                    AdultsCount = 1,
                    ChildCount = 0,
                    BabyCount = 0,
                    IsDeleted = false,
                    UserId = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                    RoomId = Guid.Parse("777634E2-3BB6-4748-8E91-7A10B70C78AC")
                }
            };

            return seedBooking;
        }
    }
}
