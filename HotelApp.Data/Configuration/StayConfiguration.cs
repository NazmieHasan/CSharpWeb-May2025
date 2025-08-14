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

            // Seed stays data with migration for development
            entity
                .HasData(this.SeedStays());
        }

        public List<Stay> SeedStays()
        {
            List<Stay> stays = new List<Stay>()
            {
                new Stay()
                {
                    Id = Guid.Parse("7b6bdc1f-e561-4fd6-bd03-d9756b60978e"),
                    CreatedOn = new DateTime(2025, 1, 5),
                    GuestId = "396a0c6d-1448-4f1f-8047-a0076620ff09",
                    BookingId = "033b12ed-6bcc-428e-897e-32db2974bd92"
                },
                new Stay()
                {
                    Id = Guid.Parse("dc801e92-aacc-4151-9448-b3d4200332b1"),
                    CreatedOn = new DateTime(2025, 1, 5),
                    GuestId = "1ad1d737-c60d-4d83-995a-3c8e42b2f236",
                    BookingId = "033b12ed-6bcc-428e-897e-32db2974bd92"
                },
                new Stay()
                {
                    Id = Guid.Parse("d1940e15-594b-42a0-bc52-0ae788fdc91e"),
                    CreatedOn = new DateTime(2025, 1, 5),
                    GuestId = "ef570e9a-b8da-4cbe-af17-60e15cea5ee3",
                    BookingId = "b89a76cb-0680-4923-b99a-40c9ce018352"
                }
            };

            return stays;
        }
    }
}
