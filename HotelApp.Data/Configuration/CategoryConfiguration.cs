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
                .HasColumnType("decimal(18,3)");

            // Define constraints for the ImageUrl column
            entity
                .Property(c => c.ImageUrl)
                .IsRequired()
                .HasMaxLength(ImageUrlMaxLength);

            // Define constraints for the IsDeleted column
            entity
                .Property(c => c.IsDeleted)
                .IsRequired()
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
                    Description = "Modern and stylish design for you",
                    Beds = 2,
                    Price = 500.00M,
                    ImageUrl = "https://cdn.pixabay.com/photo/2015/11/06/11/45/interior-1026452_960_720.jpg"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Apartment",
                    Description = "Modern design, comfort and convenience",
                    Beds = 4,
                    Price = 800.00M,
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/04/28/22/14/room-2269591_960_720.jpg"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Apartment Lux",
                    Description = "Luxury, elegance and comfort",
                    Beds = 4,
                    Price = 1500.00M,
                    ImageUrl = "https://cdn.pixabay.com/photo/2015/01/10/11/39/hotel-595121_960_720.jpg"
                }
            };

            return categories;
        }

    }
}
