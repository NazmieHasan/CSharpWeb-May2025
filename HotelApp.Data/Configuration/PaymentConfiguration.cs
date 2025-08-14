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

            // Seed payments data with migration for development
            entity
                .HasData(this.SeedPayments());
        }

        public List<Payment> SeedPayments()
        {
            List<Payment> payments = new List<Payment>()
            {
                new Payment()
                {
                    Id = Guid.Parse("7b6bdc1f-e561-4fd6-bd03-d9756b60978e"),
                    CreatedOn = new DateTime(2025, 1, 5),
                    PaymentUserFullName = "Alex Doe",
                    PaymentUserPhoneNumber = "Doe",
                    BookingId = "2a42e8c7-ba40-46e9-a6c0-7dca4751f087",
                    Amount = "500"
                }
            };

            return payments;
        }
    }
}
