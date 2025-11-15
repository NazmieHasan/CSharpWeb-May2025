namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static Common.EntityConstants.Status;

    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> entity)
        {
            // Define the primary key of the Status entity
            entity
                .HasKey(p => p.Id);

            // Define constraints for the Name column
            entity
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            // Define constraints for the IsDeleted column
            entity
                .Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Filter out only the active (non-deleted) entries
            entity
            .HasQueryFilter(p => p.IsDeleted == false);

            // Seed payment method data with migration for development
            entity
                .HasData(this.SeedStatuses());
        }

        public List<Status> SeedStatuses()
        {
            List<Status> statuses = new List<Status>()
            {
                new Status()
                {
                    Id = 1,
                    Name = "Awaiting Payment"
                },
                new Status()
                {
                    Id = 2,
                    Name = "Cancelled"
                },
                new Status()
                {
                    Id = 3,
                    Name = "For Implementation"
                },
                new Status()
                {
                    Id = 4,
                    Name = "In Progress"
                },
                new Status()
                {
                    Id = 5,
                    Name = "Done"
                },
                new Status()
                {
                    Id = 6,
                    Name = "Done - Early Check Out"
                }
            };

            return statuses;
        }
    }
}
