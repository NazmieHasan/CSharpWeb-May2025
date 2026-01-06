namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IBookingRoomRepository
        : IRepository<BookingRoom, Guid>, IAsyncRepository<BookingRoom, Guid>
    {
    }
}
