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
                .OnDelete(DeleteBehavior.Restrict); 

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
                },
                new Room
                {
                    Id = Guid.Parse("c1bd2a4a-6f9b-4daf-a0bb-4f7cdccf6101"),
                    Name = "204",
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("bfb7b7af-d533-4b7e-a7a9-69d27e0e5d47"),
                    Name = "205",
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("ee45d08c-f4d7-4b87-9ceb-6157c703a7dc"),
                    Name = "301",
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("f1e8ce5d-8c16-4bf6-9ff6-70db57fcd118"),
                    Name = "302",
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("40c553a9-f28f-4d17-bd83-92fd2c63ff91"),
                    Name = "303",
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("fbb4b2e4-7319-45a7-ac07-fe7e7345d5cb"),
                    Name = "304",
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("c6d17679-1de6-4cbb-b625-408b7bff3dc4"),
                    Name = "305",
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("7f48c43a-8c88-486b-a59e-0e89bd4453ec"),
                    Name = "401",
                    CategoryId = 3,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("e76e6f0c-e838-4f47-b836-882b7ccc6983"),
                    Name = "402",
                    CategoryId = 3,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("a93c5830-33fb-4d67-b8d8-468ed20c5efd"),
                    Name = "403",
                    CategoryId = 3,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("8ffd66e2-7938-4e62-a246-f63cd583f2de"),
                    Name = "404",
                    CategoryId = 3,
                    IsDeleted = false
                },
                new Room
                {
                    Id = Guid.Parse("d5f93a83-98f4-46a9-85da-ff2f3282e6f5"),
                    Name = "405",
                    CategoryId = 3,
                    IsDeleted = false
                }
            };

            return seedRoom;
        }


    }
}
