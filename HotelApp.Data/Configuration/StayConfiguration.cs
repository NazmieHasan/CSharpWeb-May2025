namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StayConfiguration : IEntityTypeConfiguration<Stay>
    {
        public void Configure(EntityTypeBuilder<Stay> entity)
        {
            entity
                .HasKey(s => s.Id);


            entity
                .Property(s => s.GuestId)
                .IsRequired();

            entity
                 .Property(s => s.BookingId)
                 .IsRequired();
        }
    }
}
