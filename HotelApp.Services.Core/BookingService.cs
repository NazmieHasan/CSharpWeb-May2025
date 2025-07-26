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

        public async Task<bool> DeleteBookingAsync(string? id)
        {
            Booking? bookingToDelete = await this.FindBookingByStringId(id);
            if (bookingToDelete == null)
            {
                return false;
            }

            // TODO: To be investigated when relations to Room entity are introduced
            this.dbContext.Bookings.Remove(bookingToDelete);
            await this.dbContext.SaveChangesAsync();

            return true;
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

        public async Task<DeleteBookingViewModel?> GetBookingDeleteDetailsByIdAsync(string? id)
        {
            DeleteBookingViewModel? deleteBookingViewModel = null;

            Booking? bookingToBeDeleted = await this.FindBookingByStringId(id);
            if (bookingToBeDeleted != null)
            {
                deleteBookingViewModel = new DeleteBookingViewModel()
                {
                    Id = bookingToBeDeleted.Id.ToString(),
                    CreatedOn = bookingToBeDeleted.CreatedOn,
                    DateArrival = bookingToBeDeleted.DateArrival,
                    DateDeparture = bookingToBeDeleted.DateDeparture,
                    AdultsCount = bookingToBeDeleted.AdultsCount,
                    ChildCount = bookingToBeDeleted.ChildCount,
                    BabyCount = bookingToBeDeleted.BabyCount,
                    RoomName = bookingToBeDeleted.Room.Name,
                };
            }

            return deleteBookingViewModel;
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

        public async Task<bool> SoftDeleteBookingAsync(string? id)
        {
            Booking? bookingToDelete = await this.FindBookingByStringId(id);
            if (bookingToDelete == null)
            {
                return false;
            }

            // Soft Delete <=> Edit of IsDeleted property
            bookingToDelete.IsDeleted = true;

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
