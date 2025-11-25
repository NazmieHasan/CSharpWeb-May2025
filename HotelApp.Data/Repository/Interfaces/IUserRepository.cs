namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IUserRepository
        : IRepository<ApplicationUser, Guid>, IAsyncRepository<ApplicationUser, Guid>
    {
    }
}
