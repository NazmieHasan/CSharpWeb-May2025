namespace HotelApp.Data.Repository
{
    using Interfaces;
    using Models;

    public class ManagerRepository : BaseRepository<Manager, Guid>, IManagerRepository
    {
        public ManagerRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
