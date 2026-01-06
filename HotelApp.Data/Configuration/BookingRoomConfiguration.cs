namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BookingRoomConfiguration : IEntityTypeConfiguration<BookingRoom>
    {
        public void Configure(EntityTypeBuilder<BookingRoom> entity)
        {
            entity
                .HasKey(br => br.Id);

            entity
                .Property(br => br.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(br => br.Booking)
                .WithMany(b => b.BookingRooms)
                .HasForeignKey(br => br.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(br => br.Room)
                .WithMany(r => r.BookingRooms)
                .HasForeignKey(br => br.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(br => br.Status)
                .WithMany(s => s.BookingRooms)
                .HasForeignKey(br => br.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(br => new { br.BookingId, br.RoomId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            entity
                .HasQueryFilter(br => br.IsDeleted == false);
        }
    }
}
