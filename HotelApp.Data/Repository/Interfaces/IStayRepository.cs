namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IStayRepository
        : IRepository<Stay, Guid>, IAsyncRepository<Stay, Guid>
    {
    }
}
