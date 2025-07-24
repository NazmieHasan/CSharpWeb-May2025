namespace HotelApp.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class HotelAppDbContext : IdentityDbContext
    {
        public HotelAppDbContext(DbContextOptions<HotelAppDbContext> options)
            : base(options)
        {
        }
    }
}
