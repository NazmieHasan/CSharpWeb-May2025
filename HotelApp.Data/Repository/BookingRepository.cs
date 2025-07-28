namespace HotelApp.Data.Repository
{
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BookingRepository : BaseRepository<Booking, Guid>, IBookingRepository
    {
        public BookingRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserId(string userId)
        {
            return await this
                .GetAllAttached()
                .Where(b => b.UserId.ToLower() == userId.ToLower())
                .ToListAsync();
        }

    }
}
