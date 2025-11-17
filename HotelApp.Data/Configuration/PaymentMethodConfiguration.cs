namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static Common.EntityConstants.PaymentMethod;

    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> entity)
        {
            // Define the primary key of the Payment method entity
            entity
                .HasKey(p => p.Id);

            // Define constraints for the Name column
            entity
                .Property(p => p.Name)
                .HasMaxLength(NameMaxLength);

            // Define constraints for the IsDeleted column
            entity
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            // Filter out only the active (non-deleted) entries
            entity
            .HasQueryFilter(p => p.IsDeleted == false);

            // Seed payment method data with migration for development
            entity
                .HasData(this.SeedPaymentMethods());
        }

        public List<PaymentMethod> SeedPaymentMethods()
        {
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>()
            {
                new PaymentMethod()
                {
                    Id = 1,
                    Name = "Bank Transfer"
                },
                new PaymentMethod()
                {
                    Id = 2,
                    Name = "Cashe"
                },
                new PaymentMethod()
                {
                    Id = 3,
                    Name = "Debit Card/ Credit Card"
                }
            };

            return paymentMethods;
        }

    }
}

