namespace HotelApp.Data.Repository
{
    using HotelApp.Data.Models;
    using HotelApp.Data.Repository.Interfaces;

    public class GuestRepository : BaseRepository<Guest, Guid>, IGuestRepository
    {
        public GuestRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
