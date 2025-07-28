namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface ICategoryRepository
        : IRepository<Category, int>, IAsyncRepository<Category, int>
    {
    }
}
