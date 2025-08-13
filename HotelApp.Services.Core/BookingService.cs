namespace HotelApp.Services.Core
{
    using System.Globalization;

    using Microsoft.EntityFrameworkCore;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Booking;
    using static GCommon.ApplicationConstants;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingService(IBookingRepository bookingRepository, 
            UserManager<ApplicationUser> userManager)
        {
            this.bookingRepository = bookingRepository;
            this.userManager = userManager;
        }

        public async Task<bool> AddBookingAsync(string userId, AddBookingInputModel inputModel)
        {
            bool opRes = false;

            IdentityUser? user = await this.userManager.FindByIdAsync(userId);

            if (user != null)
            {
                Booking newBooking = new Booking()
                {
                    DateArrival = inputModel.DateArrival,
                    DateDeparture = inputModel.DateDeparture,
                    AdultsCount = inputModel.AdultsCount,
                    ChildCount = inputModel.ChildCount,
                    BabyCount = inputModel.BabyCount,
                    UserId = userId,
                    RoomId = new Guid("AE50A5AB-9642-466F-B528-3CC61071BB4C")
                };

                await this.bookingRepository.AddAsync(newBooking);

                opRes = true;
            }

            return opRes;
        }

        public async Task<bool> AddBookingAsync(string userId, string arrival, string departure, int adultsCount, int childCount, int babyCount)
        {
            bool opRes = false;

            IdentityUser? user = await this.userManager.FindByIdAsync(userId);

            if (user != null)
            {
                Booking newBooking = new Booking()
                {
                    DateArrival = DateOnly.Parse(arrival),
                    DateDeparture = DateOnly.Parse(departure),
                    AdultsCount = adultsCount,
                    ChildCount = childCount,
                    BabyCount = babyCount,
                    UserId = userId,
                    RoomId = new Guid("AE50A5AB-9642-466F-B528-3CC61071BB4C")
                };

                await this.bookingRepository.AddAsync(newBooking);

                opRes = true;
            }

            return opRes;
        }

        public async Task<bool> DeleteBookingAsync(string? id)
        {
            Booking? bookingToDelete = await this.FindBookingByStringId(id);
            if (bookingToDelete == null)
            {
                return false;
            }

            // TODO: To be investigated when relations to Room entity are introduced
            await this.bookingRepository
                .HardDeleteAsync(bookingToDelete);

            return true;
        }

        public async Task<IEnumerable<AllBookingsIndexViewModel>> GetAllBookingsAsync()
        {
            IEnumerable<AllBookingsIndexViewModel> allBookings = await this.bookingRepository
                .GetAllAttached()
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
                bookingDetails = await this.bookingRepository
                    .GetAllAttached()
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
                editModel = await this.bookingRepository
                    .GetAllAttached()
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

        public async Task<IEnumerable<MyBookingsViewModel>> GetBookingsByUserIdAsync(string userId)
        {
            // Due to the use of the built-in IdentityUser, we do not have direct navigation collection from the user side
            IEnumerable<MyBookingsViewModel> myBookings = await this.bookingRepository
                .GetAllAttached()
                .Include(b => b.Room)
                .AsNoTracking()
                .Where(b => b.UserId.ToLower() == userId.ToLower())
                .Select(b => new MyBookingsViewModel()
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
                .ToArrayAsync();

            return myBookings;
        }

        public async Task<IEnumerable<string>> GetBookingsIdByUserIdAsync(string? userId)
        {
            IEnumerable<string> bookingIds = new List<string>();
            if (!String.IsNullOrWhiteSpace(userId))
            {
                bookingIds = await this.bookingRepository
                    .GetAllAttached()
                    .Where(b => b.UserId.ToString().ToLower() == userId.ToLower())
                    .Select(b => b.Id.ToString())
                    .ToArrayAsync();
            }

            return bookingIds;
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

            await this.bookingRepository.UpdateAsync(editableBooking);

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
            await this.bookingRepository.DeleteAsync(bookingToDelete);

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
                    booking = await this.bookingRepository
                    .GetByIdAsync(bookingGuid);
                }
            }

            return booking;
        }
    }
}
