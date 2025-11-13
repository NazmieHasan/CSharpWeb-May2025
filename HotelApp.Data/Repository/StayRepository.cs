namespace HotelApp.Data.Repository
{
    using HotelApp.Data.Models;
    using HotelApp.Data.Repository.Interfaces;

    public class StayRepository : BaseRepository<Stay, Guid>, IStayRepository
    {
        public StayRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
