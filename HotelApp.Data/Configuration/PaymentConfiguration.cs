namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> entity)
        {
            entity
                .HasKey(p => p.Id);

            entity
               .Property(p => p.CreatedOn)
               .HasDefaultValueSql("GETUTCDATE()");

            entity
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            entity
                .Property(p => p.PaymentUserFullName)
                .IsRequired();

            entity
                .Property(p => p.PaymentUserPhoneNumber)
                .IsRequired();

            entity
                .Property(p => p.Amount)
                .IsRequired();

            // Define constraints for the IsDeleted column
            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            // Filter out only the active (non-deleted) entries
            entity
            .HasQueryFilter(p => p.IsDeleted == false);

            entity
                .HasOne(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
