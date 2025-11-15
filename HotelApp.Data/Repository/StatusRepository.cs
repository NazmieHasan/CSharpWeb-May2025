namespace HotelApp.Data.Repository
{
    using HotelApp.Data.Models;
    using HotelApp.Data.Repository.Interfaces;

    public class StatusRepository : BaseRepository<Status, int>, IStatusRepository
    {
        public StatusRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
