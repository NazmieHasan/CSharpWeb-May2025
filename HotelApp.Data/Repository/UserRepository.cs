namespace HotelApp.Data.Repository
{
    using HotelApp.Data.Models;
    using HotelApp.Data.Repository.Interfaces;

    public class UserRepository : BaseRepository<ApplicationUser, Guid>, IUserRepository
    {
        public UserRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
