namespace HotelApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> entity)
        {
            // rename table, beacause in database name is Manager 
            entity.ToTable("Managers");

            entity
                .HasKey(m => m.Id);

            entity
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(m => m.User)
                .WithOne()
                .HasForeignKey<Manager>(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasIndex(m => new { m.UserId })
                .IsUnique();

            entity
                .HasQueryFilter(m => m.IsDeleted == false);
        }
    }
}
