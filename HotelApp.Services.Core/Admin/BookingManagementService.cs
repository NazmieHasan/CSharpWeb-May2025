namespace HotelApp.Services.Core.Admin
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.BookingManagement;
    using Web.ViewModels.Admin.StayManagement;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

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

        public async Task<IEnumerable<BookingManagementIndexViewModel>> GetBookingManagementBoardDataAsync()
        {
            return await bookingRepository
               .GetAllAttached()
               .IgnoreQueryFilters()
               .AsNoTracking()
               .Include(b => b.User)
               .OrderByDescending(b => b.CreatedOn)
               .Select(b => new BookingManagementIndexViewModel
               {
                   Id = b.Id.ToString(),
                   CreatedOn = b.CreatedOn,
                   UserEmail = b.User.Email,
                   ManagerEmail = b.Manager != null ?
                        b.Manager.User.UserName : null,
                   IsDeleted = b.IsDeleted
               })
               .ToListAsync()
               ?? Enumerable.Empty<BookingManagementIndexViewModel>();
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
                .AsNoTracking()
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
                        GuestEmail = s.Guest.Email,
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
                        StatusId = bookingToEdit.StatusId
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
    }
}
