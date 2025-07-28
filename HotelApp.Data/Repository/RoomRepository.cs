namespace HotelApp.Data.Repository
{
    using Interfaces;
    using Models;

    public class RoomRepository : BaseRepository<Room, Guid>, IRoomRepository
    {
        public RoomRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
