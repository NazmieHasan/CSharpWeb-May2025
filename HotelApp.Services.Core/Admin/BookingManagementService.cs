namespace HotelApp.Services.Core.Admin
{
    using Data.Models;
    using Data.Repository.Interfaces;
    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;
    using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Admin.BookingManagement;
    using Web.ViewModels.Admin.StayManagement;

    public class BookingManagementService : IBookingManagementService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IManagerRepository managerRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStatusManagementService statusService;

        public BookingManagementService(IBookingRepository bookingRepository,
            IManagerRepository managerRepository,
            UserManager<ApplicationUser> userManager,
            IStatusManagementService statusService)
        {
            this.bookingRepository = bookingRepository;
            this.managerRepository = managerRepository;
            this.userManager = userManager;
            this.statusService = statusService;
        }

        public async Task<Booking?> FindBookingByIdAsync(Guid id)
        {
            return await this.bookingRepository
                .GetAllAttached()
                .Include(b => b.Room)                     
                    .ThenInclude(r => r.Category)        
                .Include(b => b.Status)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BookingManagementIndexViewModel>> GetBookingManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize)
        {
            var query = this.bookingRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Include(b => b.Status)
                .AsNoTracking()
                .OrderByDescending(b => b.CreatedOn)
                .Select(b => new BookingManagementIndexViewModel
                {
                    Id = b.Id.ToString(),
                    CreatedOn = b.CreatedOn,
                    DateArrival = b.DateArrival,
                    DateDeparture = b.DateDeparture,
                    Status = b.Status.Name,
                    IsDeleted = b.IsDeleted
                });

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalBookingsCountAsync()
        {
            return await bookingRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .CountAsync();
        }

        public async Task<BookingManagementDetailsViewModel?> GetBookingManagementDetailsByIdAsync(string? id)
        {
            BookingManagementDetailsViewModel? bookingDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid bookingId);

            if (!isIdValidGuid)
            {
                return null;
            }

            bookingDetails = await this.bookingRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Include(b => b.User)
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Include(b => b.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(b => b.Status)
                .Include(b => b.Stays)
                    .ThenInclude(s => s.Guest)
                .Where(b => b.Id == bookingId)
                .Select(b => new BookingManagementDetailsViewModel()
                {
                    Id = b.Id.ToString(),
                    CreatedOn = b.CreatedOn,
                    Status = b.Status.Name,
                    DateArrival = b.DateArrival,
                    DateDeparture = b.DateDeparture,
                    DaysCount = b.DaysCount,
                    AdultsCount = b.AdultsCount,
                    ChildCount = b.ChildCount,
                    BabyCount = b.BabyCount,
                    UserEmail = b.User.Email,
                    ManagerEmail = b.Manager != null ? b.Manager.User.UserName : null,
                    RoomId = b.Room.Id.ToString(),
                    Room = b.Room.Name,
                    RoomCategory = b.Room.Category.Name,
                    AllowedGuestCount = b.AdultsCount + b.ChildCount + b.BabyCount,
                    TotalAmount = b.TotalAmount,
                    PaidAmount = b.Payments.Sum(p => p.Amount),
                    RemainingAmount = b.TotalAmount - b.Payments.Sum(p => p.Amount),
                    IsDeleted = b.IsDeleted,
                    Payments = b.Payments.Select(p => new PaymentManagementDetailsViewModel
                    {
                        Id = p.Id,
                        CreatedOn = p.CreatedOn,
                        Amount = p.Amount,
                        PaymentUserFullName = p.PaymentUserFullName,
                        PaymentUserPhoneNumber = p.PaymentUserPhoneNumber,
                        IsDeleted = p.IsDeleted,
                        PaymentMethodName = p.PaymentMethod.Name
                    }).ToList(),
                    Stays = b.Stays.Select(s => new StayManagementDetailsViewModel
                    {
                        Id = s.Id,
                        GuestFirstName = s.Guest.FirstName,
                        GuestFamilyName = s.Guest.FamilyName,
                        CreatedOn = s.CreatedOn,
                        CheckoutOn = s.CheckoutOn,
                        IsDeleted = s.IsDeleted
                    }).ToList()
                })
                .SingleOrDefaultAsync();

            if (bookingDetails != null)
            {
                if (bookingDetails.IsDeleted)
                {
                    bookingDetails.AllowedOperation = "None";
                }
                else if (bookingDetails.Status == "Awaiting Payment")
                {
                    bookingDetails.AllowedOperation = "AddPayment";
                }
                else if (bookingDetails.Status == "For Implementation" && DateOnly.FromDateTime(DateTime.UtcNow) >= bookingDetails.DateArrival)
                {
                    bookingDetails.AllowedOperation = "AddStay";
                }
                else if (bookingDetails.Status == "In Progress" && bookingDetails.AllowedGuestCount > bookingDetails.Stays.Count())
                {
                    bookingDetails.AllowedOperation = "AddStay";
                }
                else
                {
                    bookingDetails.AllowedOperation = "None";
                }
            }

            return bookingDetails;
        }

        public async Task<BookingManagementEditFormModel?> GetBookingEditFormModelAsync(string? id)
        {
            BookingManagementEditFormModel? formModel = null;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Booking? bookingToEdit = await this.bookingRepository
                    .GetAllAttached()
                    .Include(b => b.Room)         
                        .ThenInclude(r => r.Category)
                    .Include(b => b.Manager)
                        .ThenInclude(m => m.User)
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(b => b.Id.ToString().ToLower() == id.ToLower());
                if (bookingToEdit != null)
                {
                    formModel = new BookingManagementEditFormModel()
                    {
                        Id = bookingToEdit.Id.ToString(),
                        AdultsCount = bookingToEdit.AdultsCount,
                        ChildCount = bookingToEdit.ChildCount,
                        BabyCount = bookingToEdit.BabyCount,
                        DateDeparture = bookingToEdit.DateDeparture,
                        ManagerEmail = bookingToEdit.Manager != null ?
                            bookingToEdit.Manager.User.Email ?? string.Empty : string.Empty,
                        StatusId = bookingToEdit.StatusId,
                        MaxGuests = bookingToEdit.Room.Category.Beds
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
                        .SingleOrDefaultAsync(b => b.Id.ToString().ToLower() == inputModel.Id.ToLower());
                    
                    if (manager != null &&
                        bookingToEdit != null)
                    {
                        var allowedStatuses = await this.statusService
                            .GetAllowedStatusesAsync(bookingToEdit.StatusId, bookingToEdit.DateDeparture, bookingToEdit.Id.ToString());

                        if (!allowedStatuses.Any(s => s.Id == inputModel.StatusId))
                        {
                            return false;
                        }

                        bookingToEdit.AdultsCount = inputModel.AdultsCount;
                        bookingToEdit.ChildCount = inputModel.ChildCount;
                        bookingToEdit.BabyCount = inputModel.BabyCount;
                        bookingToEdit.Manager = manager;
                        bookingToEdit.StatusId = inputModel.StatusId;

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

        public async Task<IEnumerable<BookingManagementSearchResultViewModel>> SearchBookingAsync(BookingManagementSearchInputModel inputModel)
        {
            var query = bookingRepository
                .GetAllAttached()
                .Include(b => b.Status)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AsQueryable();

            // Booking Id
            if (!string.IsNullOrWhiteSpace(inputModel.Id)
                && Guid.TryParse(inputModel.Id, out Guid bookingId))
            {
                query = query.Where(b => b.Id == bookingId);
            }

            // Created On
            if (inputModel.CreatedOn.HasValue)
            {
                var createdDate = inputModel.CreatedOn.Value.Date;

                query = query.Where(b =>
                    b.CreatedOn.Date == createdDate);
            }

            // Date Arrival
            if (inputModel.DateArrival != default)
            {
                query = query.Where(b =>
                    b.DateArrival == inputModel.DateArrival);
            }

            // Date Departure
            if (inputModel.DateDeparture != default)
            {
                query = query.Where(b =>
                    b.DateDeparture == inputModel.DateDeparture);
            }

            // Status
            if (inputModel.StatusId.HasValue)
            {
                query = query.Where(b =>
                    b.StatusId == inputModel.StatusId.Value);
            }

            // IsDeleted
            if (inputModel.IsDeleted.HasValue)
            {
                query = query.Where(b =>
                    b.IsDeleted == inputModel.IsDeleted.Value);
            }

            var bookings = await query
                .OrderByDescending(b => b.CreatedOn)
                .Select(b => new BookingManagementSearchResultViewModel
                {
                    Id = b.Id.ToString(),
                    CreatedOn = b.CreatedOn,
                    DateArrival = b.DateArrival,
                    DateDeparture = b.DateDeparture,
                    Status = b.Status.Name,
                    IsDeleted = b.IsDeleted
                })
                .ToListAsync();

            return bookings;
        }

    }
}
