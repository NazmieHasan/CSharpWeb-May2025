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

            // Seed guets data with migration for development
            entity
                .HasData(this.SeedGuests());
        }

        public List<Guest> SeedGuests()
        {
            List<Guest> guests = new List<Guest>()
            {
                new Guest()
                {
                    Id = Guid.Parse("5d041311-f1b6-44c9-b453-de3c3ad4a7c4"),
                    CreatedOn = new DateTime(2025, 1, 5),
                    FirstName = "John",
                    FamilyName = "Doe",
                    PhoneNumber = "+111122222"
                },
                new Guest()
                {
                    Id = Guid.Parse("ad35b73a-9686-4df6-a2d6-210b757370ab"),
                    CreatedOn = new DateTime(2025, 2, 10),
                    FirstName = "Jane",
                    FamilyName = "Smith",
                    PhoneNumber = "+222233333"
                }
            };

            return guests;
        }
    }
}
