namespace HotelApp.Services.Core
{
    using HotelApp.Data;
    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BookingService : IBookingService
    {
        private readonly HotelAppDbContext dbContext;

        public BookingService(HotelAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AllBookingsIndexViewModel>> GetAllBookingsAsync()
        {
            IEnumerable<AllBookingsIndexViewModel> allBookings = await this.dbContext
                .Bookings
                .Include(b => b.Room)
                .Where(b => !b.IsDeleted)
                .Select(b => new AllBookingsIndexViewModel
                {
                    Id = b.Id.ToString(),
                    Room = b.Room.Name,
                    RoomId = b.RoomId.ToString(),
                    CreatedOn = b.CreatedOn,
                    DateArrival = b.DateArrival,
                    DateDeparture = b.DateDeparture,
                    AdultsCount = b.AdultsCount,
                    ChildCount = b.ChildCount,
                    BabyCount = b.BabyCount,
                })
                .ToArrayAsync();

            return allBookings;
        }
    }
}
