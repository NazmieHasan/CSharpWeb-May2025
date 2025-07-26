namespace HotelApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Interfaces;
    using HotelApp.Web.ViewModels.Booking;


    public class BookingService : IBookingService
    {
        private readonly HotelAppDbContext dbContext;

        public BookingService(HotelAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddBookingAsync(AddBookingInputModel inputModel)
        {
            Booking newBooking = new Booking()
            {
                DateArrival = inputModel.DateArrival,
                DateDeparture = inputModel.DateDeparture,
                AdultsCount = inputModel.AdultsCount,
                ChildCount = inputModel.ChildCount,
                BabyCount = inputModel.BabyCount,
                RoomId = new Guid("AE50A5AB-9642-466F-B528-3CC61071BB4C")
            };

            await this.dbContext.AddAsync(newBooking);
            await this.dbContext.SaveChangesAsync();
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
