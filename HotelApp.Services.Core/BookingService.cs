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

        public async Task<BookingDetailsViewModel?> GetBookingDetailsByIdAsync(string? id)
        {
            BookingDetailsViewModel? bookingDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid bookingId);

            if (isIdValidGuid)
            {
                bookingDetails = await this.dbContext
                    .Bookings
                    .Include(b => b.Room)
                    .AsNoTracking()
                    .Where(b => b.Id == bookingId)
                    .Select(b => new BookingDetailsViewModel()
                    {
                        Id = b.Id.ToString(),
                        CreatedOn = b.CreatedOn,
                        DateArrival = b.DateArrival,
                        DateDeparture = b.DateDeparture,
                        AdultsCount = b.AdultsCount,
                        ChildCount = b.ChildCount,
                        BabyCount = b.BabyCount,
                        Room = b.Room.Name
                    })
                    .SingleOrDefaultAsync();
            }

            return bookingDetails;
        }

        public async Task<EditBookingInputModel?> GetBookingForEditAsync(string? id)
        {
            EditBookingInputModel? editModel = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid bookingId);

            if (isIdValidGuid)
            {
                editModel = await this.dbContext
                    .Bookings
                    .AsNoTracking()
                    .Where(b => b.Id == bookingId)
                    .Select(b => new EditBookingInputModel()
                    {
                        Id = b.Id.ToString(),
                        AdultsCount = b.AdultsCount,
                        ChildCount = b.ChildCount,
                        BabyCount = b.BabyCount
                    })
                    .SingleOrDefaultAsync();
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedBookingAsync(EditBookingInputModel inputModel)
        {
            Booking? editableBooking = await this.FindBookingByStringId(inputModel.Id);

            if (editableBooking == null)
            {
                return false;
            }

            editableBooking.AdultsCount = inputModel.AdultsCount;
            editableBooking.ChildCount = inputModel.ChildCount;
            editableBooking.BabyCount = inputModel.BabyCount;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        // TODO: Implement as generic method in BaseService
        private async Task<Booking?> FindBookingByStringId(string? id)
        {
            Booking? booking = null;

            if (!string.IsNullOrWhiteSpace(id))
            {
                bool isGuidValid = Guid.TryParse(id, out Guid bookingGuid);
                if (isGuidValid)
                {
                    booking = await this.dbContext
                        .Bookings
                        .Include(b => b.Room)
                        .FirstOrDefaultAsync(b => b.Id == bookingGuid);
                }
            }

            return booking;
        }
    }
}
