namespace HotelApp.Services.Core.Admin
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.BookingManagement;

    public class BookingManagementService : IBookingManagementService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IManagerRepository managerRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingManagementService(IBookingRepository bookingRepository,
            IManagerRepository managerRepository, UserManager<ApplicationUser> userManager)
        {
            this.bookingRepository = bookingRepository;
            this.managerRepository = managerRepository;
            this.userManager = userManager;
        }

        public async Task<BookingManagementEditFormModel?> GetBookingForEditAsync(string? id)
        {
            BookingManagementEditFormModel? editModel = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid bookingId);

            if (isIdValidGuid)
            {
                editModel = await this.bookingRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Include(b => b.Manager)
                    .ThenInclude(m => m.User)
                    .IgnoreQueryFilters()
                    .Where(b => b.Id == bookingId)
                    .Select(b => new BookingManagementEditFormModel()
                    {
                        Id = b.Id.ToString(),
                        AdultsCount = b.AdultsCount,
                        ChildCount = b.ChildCount,
                        BabyCount = b.BabyCount,
                        ManagerEmail = b.Manager != null ?
                            b.Manager.User.Email ?? string.Empty : string.Empty,
                    })
                    .SingleOrDefaultAsync();
            }

            return editModel;
        }

        public async Task<IEnumerable<BookingManagementIndexViewModel>> GetBookingManagementBoardDataAsync()
        {
            return await bookingRepository
               .GetAllAttached()
               .IgnoreQueryFilters()
               .AsNoTracking()
               .Include(b => b.Room)
               .OrderByDescending(b => b.CreatedOn)
               .Select(b => new BookingManagementIndexViewModel
               {
                   Id = b.Id.ToString(),
                   Room = b.Room.Name,
                   RoomId = b.RoomId.ToString(),
                   CreatedOn = b.CreatedOn,
                   DateArrival = b.DateArrival,
                   DateDeparture = b.DateDeparture,
                   IsDeleted = b.IsDeleted,
                   ManagerName = b.Manager != null ?
                        b.Manager.User.UserName : null,
               })
               .ToListAsync()
               ?? Enumerable.Empty<BookingManagementIndexViewModel>();
        }

        public async Task<bool> PersistUpdatedBookingAsync(BookingManagementEditFormModel inputModel)
        {
            Booking? editableBooking = await this.FindBookingByStringId(inputModel.Id);

            if (editableBooking == null)
            {
                return false;
            }

            Manager? manager = null;

            if (!string.IsNullOrWhiteSpace(inputModel.ManagerEmail))
            {
                ApplicationUser? managerUser = await this.userManager.FindByNameAsync(inputModel.ManagerEmail);

                if (managerUser != null)
                {
                    manager = await this.managerRepository
                        .GetAllAttached()
                        .SingleOrDefaultAsync(m => m.UserId.ToLower() == managerUser.Id.ToLower());
                }
            }

            editableBooking.AdultsCount = inputModel.AdultsCount;
            editableBooking.ChildCount = inputModel.ChildCount;
            editableBooking.BabyCount = inputModel.BabyCount;
            editableBooking.Manager = manager;

            await this.bookingRepository.UpdateAsync(editableBooking);

            return true;
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreBookingAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Booking? booking = await this.bookingRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(b => b.Id.ToString().ToLower() == id.ToLower());
                if (booking != null)
                {
                    if (booking.IsDeleted)
                    {
                        isRestored = true;
                    }

                    booking.IsDeleted = !booking.IsDeleted;

                    result = await this.bookingRepository
                        .UpdateAsync(booking);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

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
