namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IStatusRepository
        : IRepository<Status, int>, IAsyncRepository<Status, int>
    {
    }
}
