namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static Common.EntityConstants.Room;

    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> entity)
        {
            entity
                .HasKey(r => r.Id);

            entity
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(r => r.IsDeleted == false);

            entity
                .HasOne(c => c.Category)
                .WithMany(r => r.Rooms)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); ;

            entity
                .HasData(this.GenerateSeedRoom());
        }

        private List<Room> GenerateSeedRoom()
        {
            List<Room> seedRoom = new List<Room>()
            {
                new Room
                {
                    Id = Guid.Parse("ae50a5ab-9642-466f-b528-3cc61071bb4c"),
                    Name = "201",
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("777634e2-3bb6-4748-8e91-7a10b70c78ac"),
                    Name = "202",
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("68fb84b9-ef2a-402f-b4fc-595006f5c275"),
                    Name = "203",
                    CategoryId = 1,
                    IsDeleted = false
                }
            };

            return seedRoom;
        }


    }
}
