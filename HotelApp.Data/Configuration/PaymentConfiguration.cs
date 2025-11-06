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
                .Property(p => p.PaymentUserFullName)
                .IsRequired();

            entity
                .Property(p => p.PaymentUserPhoneNumber)
                .IsRequired();

            entity
                .Property(p => p.Amount)
                .IsRequired();

            entity
                .Property(p => p.BookingId)
                .IsRequired();
        }
    }
}
