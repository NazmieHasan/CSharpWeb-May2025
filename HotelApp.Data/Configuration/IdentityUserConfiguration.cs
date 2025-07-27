namespace HotelApp.Data.Configuration
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> entity)
        {
            entity
                .HasData(this.CreateDefaultAdminUser());
        }

        private IdentityUser CreateDefaultAdminUser()
        {
            IdentityUser defaultUser = new IdentityUser
            {
                Id = "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                UserName = "admin@hotelsystem.com",
                NormalizedUserName = "ADMIN@HOTELSYSTEM.COM",
                Email = "admin@hotelsystem.com",
                NormalizedEmail = "ADMIN@HOTELSYSTEM.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                    new IdentityUser { UserName = "admin@hotelsystem.com" }, "Admin123!")
            };

            return defaultUser;
        }
    }
}
