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

        public async Task<BookingManagementEditFormModel?> GetBookingEditFormModelAsync(string? id)
        {
            BookingManagementEditFormModel? formModel = null;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Booking? bookingToEdit = await this.bookingRepository
                    .GetAllAttached()
                    .Include(b => b.Manager)
                    .ThenInclude(m => m.User)
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(b => b.Id.ToString().ToLower() == id.ToLower());
                if (bookingToEdit != null)
                {
                    formModel = new BookingManagementEditFormModel()
                    {
                        Id = bookingToEdit.Id.ToString(),
                        AdultsCount = bookingToEdit.AdultsCount,
                        ChildCount = bookingToEdit.ChildCount,
                        BabyCount = bookingToEdit.BabyCount,
                        ManagerEmail = bookingToEdit.Manager != null ?
                            bookingToEdit.Manager.User.Email ?? string.Empty : string.Empty,
                    };
                }
            }

            return formModel;
        }

        public async Task<bool> EditBookingAsync(BookingManagementEditFormModel? inputModel)
        {
            bool result = false;
            if (inputModel != null)
            {
                ApplicationUser? managerUser = await this.userManager
                    .FindByNameAsync(inputModel.ManagerEmail);
                if (managerUser != null)
                {
                    Manager? manager = await this.managerRepository
                        .GetAllAttached()
                        .SingleOrDefaultAsync(m => m.UserId.ToLower() == managerUser.Id.ToLower());
                    Booking? bookingToEdit = await this.bookingRepository
                        .SingleOrDefaultAsync(c => c.Id.ToString().ToLower() == inputModel.Id.ToLower());
                    if (manager != null &&
                        bookingToEdit != null)
                    {
                        bookingToEdit.AdultsCount = inputModel.AdultsCount;
                        bookingToEdit.ChildCount = inputModel.ChildCount;
                        bookingToEdit.BabyCount = inputModel.BabyCount;
                        bookingToEdit.Manager = manager;

                        result = await this.bookingRepository
                            .UpdateAsync(bookingToEdit);
                    }
                }
            }

            return result;
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
    }
}
