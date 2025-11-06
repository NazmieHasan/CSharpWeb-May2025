namespace HotelApp.Data.Repository.Interfaces
{ 
    using Models;

    public interface IGuestRepository
        : IRepository<Guest, Guid>, IAsyncRepository<Guest, Guid>
    {
    } 
}
