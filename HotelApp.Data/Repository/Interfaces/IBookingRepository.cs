namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IBookingRepository
        : IRepository<Booking, Guid>, IAsyncRepository<Booking, Guid>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserId(string userId);
    }

}
