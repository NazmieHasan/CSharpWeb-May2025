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
               .Property(b => b.CreatedOn)
               .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(s => s.CheckoutOn)
                .IsRequired(false);

            // Define constraints for the IsDeleted column
            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity.HasOne(s => s.Booking)
                  .WithMany(b => b.Stays)
                  .HasForeignKey(s => s.BookingId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Guest)
                  .WithMany(g => g.Stays)
                  .HasForeignKey(s => s.GuestId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Filter out only the active (non-deleted) entries
            entity
            .HasQueryFilter(p => p.IsDeleted == false);
        }
    }
}
