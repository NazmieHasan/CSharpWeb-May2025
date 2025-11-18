namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> entity)
        {
            // Define the primary key of the Guest entity
            entity
                .HasKey(g => g.Id);

            entity
                .Property(b => b.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            // Define constraints for the First Name column
            entity
                .Property(g => g.FirstName)
                .IsRequired();

            // Define constraints for the Family Name column
            entity
                .Property(g => g.FamilyName)
                .IsRequired();

            // Define constraints for the First Name column
            entity
                .Property(g => g.PhoneNumber)
                .IsRequired();

            // Define constraints for the Email column
            entity
                .Property(g => g.Email)
                .IsRequired();

            entity
                .Property(b => b.IsDeleted)
                .HasDefaultValue(false);

            // Filter out only the active (non-deleted) entries
            entity
            .HasQueryFilter(g => g.IsDeleted == false);
        }

    }
}
