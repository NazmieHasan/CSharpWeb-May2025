namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IRoomRepository
        : IRepository<Room, Guid>, IAsyncRepository<Room, Guid>
    {

    }
}
