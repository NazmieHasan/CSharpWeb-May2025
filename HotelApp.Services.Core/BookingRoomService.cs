namespace HotelApp.Services.Core
{
    using HotelApp.Data.Repository.Interfaces;
    using HotelApp.Services.Core.Interfaces;

    public class BookingRoomService : IBookingRoomService
    {
        private readonly IBookingRoomRepository bookingRoomRepository;

        public BookingRoomService(IBookingRoomRepository bookingRoomRepository)
        {
            this.bookingRoomRepository = bookingRoomRepository;
        }

    }
}
