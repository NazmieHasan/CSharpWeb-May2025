namespace HotelApp.Data.Configuration
{
    using HotelApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static Common.EntityConstants.Category;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            // Define the primary key of the Category entity
            entity
                .HasKey(c => c.Id);

            // Define constraints for the Name column
            entity
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            // Define constraints for the Description column
            entity
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            // Define constraints for the Beds column
            entity
                .Property(c => c.Beds)
                .IsRequired();

            entity
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            // Define constraints for the ImageUrl column
            entity
                .Property(c => c.ImageUrl)
                .IsRequired()
                .HasMaxLength(ImageUrlMaxLength);

            // Define constraints for the IsDeleted column
            entity
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            // Filter out only the active (non-deleted) entries
            entity
            .HasQueryFilter(c => c.IsDeleted == false);

            // Seed categories data with migration for development
            entity
                .HasData(this.SeedCategories());
        }

        public List<Category> SeedCategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Double Room",
                    Description = "Modern and stylish design",
                    Beds = 2,
                    Price = 300.00M,
                    ImageUrl = "/images/upload/categories/7fb46e46-d4d6-41b8-8b54-9c81096462f1.jpg"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Family Room",
                    Description = "Convenience for all the family",
                    Beds = 3,
                    Price = 500.00M,
                    ImageUrl = "/images/upload/categories/3ce40dc4-f2a7-4cf1-97e6-3d9e68f892fe.jpg"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Apartment",
                    Description = "Modern design, comfort and convenience",
                    Beds = 4,
                    Price = 1000.00M,
                    ImageUrl = "/images/upload/categories/b77081e5-111e-4220-a122-597e46708efd.jpg"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Double Room L",
                    Description = "Luxury, elegance and comfort",
                    Beds = 2,
                    Price = 400.00M,
                    ImageUrl = "/images/upload/categories/e3d7e66a-aebd-45d7-9ad4-04f65c2704f6.jpg"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Apartment L",
                    Description = "Luxury, elegance and comfort",
                    Beds = 4,
                    Price = 1500.00M,
                    ImageUrl = "/images/upload/categories/44e74722-5f1d-4736-bf28-bc7d914a9192.jpg"
                },
                new Category()
                {
                    Id = 6,
                    Name = "Apartment Super L",
                    Description = "Super luxury, elegance and comfort",
                    Beds = 4,
                    Price = 2000.00M,
                    ImageUrl = "/images/upload/categories/7aa43c19-831a-4890-bd6e-dded11e88d3f.jpg"
                }
            };

            return categories;
        }

    }
}
