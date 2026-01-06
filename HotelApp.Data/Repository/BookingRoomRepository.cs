namespace HotelApp.Data.Repository
{
    using Interfaces;
    using Models;

    public class BookingRoomRepository : BaseRepository<BookingRoom, Guid>, IBookingRoomRepository
    {
        public BookingRoomRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {
            
        }
    }
}
