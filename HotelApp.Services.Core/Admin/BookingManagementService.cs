namespace HotelApp.Services.Core.Admin
{
    using Data.Models;
    using Data.Repository.Interfaces;
    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Admin.BookingManagement.Report;
    using HotelApp.Web.ViewModels.Admin.BookingManagement.Search;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;
    using Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Admin.BookingManagement;
    using Web.ViewModels.Admin.StayManagement;

    using HotelApp.Services.Common.Extensions;
    using static HotelApp.GCommon.ApplicationConstants;

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
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Room)
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
                    CreatedOn = b.CreatedOn.ToHotelTime(),
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
                .AsSplitQuery()
                .Include(b => b.User)
                .Include(b => b.Manager)
                    .ThenInclude(m => m.User)
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Room)
                        .ThenInclude(r => r.Category)
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Status)
                .Include(b => b.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(b => b.Status)
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Stays)
                        .ThenInclude(s => s.Guest)
                .Where(b => b.Id == bookingId)
                .Select(b => new BookingManagementDetailsViewModel()
                {
                    Id = b.Id.ToString(),
                    CreatedOn = b.CreatedOn.ToHotelTime(),
                    Status = b.Status.Name,
                    DateArrival = b.DateArrival,
                    DateDeparture = b.DateDeparture,
                    DaysCount = b.DaysCount,
                    AdultsCount = b.AdultsCount,
                    ChildCount = b.ChildCount,
                    BabyCount = b.BabyCount,
                    UserEmail = b.User.Email,
                    Owner = b.Owner,
                    IsForAnotherPerson = b.IsForAnotherPerson,
                    ManagerEmail = b.Manager != null ? b.Manager.User.UserName : null,
                    Rooms = b.BookingRooms.Select(br => new RoomInfoInBookingManagementViewModel
                    {
                        RoomId = br.Room.Id.ToString(),
                        RoomName = br.Room.Name,
                        RoomCategory = br.Room.Category.Name,
                        AdultsCountPerRoom = br.AdultsCount,
                        ChildCountPerRoom = br.ChildCount,
                        BabyCountPerRoom = br.BabyCount,
                        RoomStatus = br.Status.Name,
                        BookingRoomId = br.Id,
                        IsAllowedAddStay = (br.StatusId == 3 && DateOnly.FromDateTime(DateTime.UtcNow) >= b.DateArrival && DateTime.UtcNow.ToHotelTime() < b.DateDeparture.ToDateTime(new TimeOnly(11, 0)))
                           || (br.StatusId == 4 && br.AdultsCount + br.ChildCount + br.BabyCount > br.Stays.Count)
                    })
                    .ToList(),
                    AllowedGuestCount = b.BookingRooms.Sum(br => br.AdultsCount + br.ChildCount + br.BabyCount),
                    TotalAmount = b.TotalAmount,
                    PaidAmount = b.Payments.Sum(p => p.Amount),
                    RemainingAmount = b.TotalAmount - b.Payments.Sum(p => p.Amount),
                    IsDeleted = b.IsDeleted,
                    Payments = b.Payments.Select(p => new PaymentManagementDetailsViewModel
                    {
                        Id = p.Id,
                        CreatedOn = p.CreatedOn.ToHotelTime(),
                        Amount = p.Amount,
                        PaymentUserFullName = p.PaymentUserFullName,
                        PaymentUserPhoneNumber = p.PaymentUserPhoneNumber,
                        IsDeleted = p.IsDeleted,
                        PaymentMethodName = p.PaymentMethod.Name
                    }).ToList(),
                    Stays = b.BookingRooms.SelectMany(br => br.Stays).Select(s => new StayManagementDetailsViewModel
                    {
                        Id = s.Id,
                        GuestFirstName = s.Guest.FirstName,
                        GuestFamilyName = s.Guest.FamilyName,
                        GuestAge = s.Guest.Age,
                        CreatedOn = s.CreatedOn.ToHotelTime(),
                        CheckoutOn = s.CheckoutOn.HasValue
                            ? s.CheckoutOn.Value.ToHotelTime()
                            : null,
                        IsDeleted = s.IsDeleted,
                        BookingRoomId = s.BookingRoomId,
                        RoomName = s.BookingRoom.Room.Name
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
                    .AsNoTracking()
                    .SingleOrDefaultAsync(b => b.Id.ToString().ToLower() == id.ToLower());
                if (bookingToEdit != null)
                {
                    formModel = new BookingManagementEditFormModel()
                    {
                        Id = bookingToEdit.Id.ToString(),
                        DateDeparture = bookingToEdit.DateDeparture,
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
                        .SingleOrDefaultAsync(b => b.Id.ToString().ToLower() == inputModel.Id.ToLower());
                    
                    if (manager != null &&
                        bookingToEdit != null)
                    {
                        var allowedStatuses = await this.statusService
                            .GetAllowedStatusesInBookingEditAsync(bookingToEdit.StatusId, bookingToEdit.DateDeparture, bookingToEdit.Id.ToString());

                        if (!allowedStatuses.Any(s => s.Id == inputModel.StatusId))
                        {
                            return false;
                        }

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
            if (!string.IsNullOrWhiteSpace(inputModel.Id))
            {
                if (!Guid.TryParse(inputModel.Id, out Guid bookingId))
                {
                    return new List<BookingManagementSearchResultViewModel>();
                }
                query = query.Where(b => b.Id == bookingId);
            }

            // Booking Owner
            if (!string.IsNullOrWhiteSpace(inputModel.Owner))
            {
                query = query.Where(b => b.Owner == inputModel.Owner);
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

        public async Task<IEnumerable<BookingManagementReportRevenueSearchResultViewModel>> ReportBookingRevenueAsync(BookingManagementReportSearchInputModel inputModel)
        {
            var bookings = await bookingRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(b => b.Status)
                .Include(b => b.Payments)
                .Where(b =>
                    b.StatusId != 1 &&
                    b.Payments.Any(p => 
                        p.Amount > 0 &&
                        p.CreatedOn.Year == inputModel.Year &&
                        p.CreatedOn.Month == inputModel.Month
                    )
                )
                .OrderBy(b => b.CreatedOn)
                .Select(b => new BookingManagementReportRevenueSearchResultViewModel
                {
                    Id = b.Id.ToString(),
                    CreatedOn = b.CreatedOn.ToString(AppDateFormat),
					DateArrival = b.DateArrival.ToString(AppDateFormat),
					DateDeparture = b.DateDeparture.ToString(AppDateFormat),
                    Status = b.Status.Name,
                    PaidAmount = b.Payments.Sum(p => p.Amount)
                })
                .ToListAsync();

            return bookings;
        }

        public async Task<IEnumerable<BookingManagementReportGuestCountSearchResultViewModel>> ReportBookingGuestCountAsync(BookingManagementReportSearchInputModel inputModel)
        {
            var monthStart = new DateOnly(inputModel.Year, inputModel.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var bookings = await bookingRepository
                .GetAllAttached()
                .AsNoTracking()
                .Include(b => b.Status)
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Room)
                        .ThenInclude(r => r.Category)
                .Where(b =>
                    b.StatusId != 1 &&
                    b.StatusId != 2 &&
                    b.DateArrival <= monthEnd &&
                    b.DateDeparture >= monthStart
                )
                .ToListAsync();

            var results = new List<BookingManagementReportGuestCountSearchResultViewModel>();

            for (var day = monthStart; day <= monthEnd; day = day.AddDays(1))
            {
                int breakfastAdults = 0; int breakfastChildren = 0; int breakfastBabies = 0;
                int lunchAdults = 0;     int lunchChildren = 0;     int lunchBabies = 0;
                int dinnerAdults = 0;    int dinnerChildren = 0;    int dinnerBabies = 0;

                for (int i = bookings.Count - 1; i >= 0; i--)
                {
                    var b = bookings[i];

                    if (day == b.DateArrival)
                    {
                        dinnerAdults += b.AdultsCount;
                        dinnerChildren += b.ChildCount;
                        dinnerBabies += b.BabyCount;
                    }
                    else if (day == b.DateDeparture)
                    {
                        breakfastAdults += b.AdultsCount;
                        breakfastChildren += b.ChildCount;
                        breakfastBabies += b.BabyCount;
                    }
                    else if (day > b.DateArrival && day < b.DateDeparture)
                    {
                        breakfastAdults += b.AdultsCount;
                        breakfastChildren += b.ChildCount;
                        breakfastBabies += b.BabyCount;

                        lunchAdults += b.AdultsCount;
                        lunchChildren += b.ChildCount;
                        lunchBabies += b.BabyCount;

                        dinnerAdults += b.AdultsCount;
                        dinnerChildren += b.ChildCount;
                        dinnerBabies += b.BabyCount;
                    }

                    if (b.DateDeparture <= day)
                    {
                        bookings.RemoveAt(i);
                    }
                }

                if (breakfastAdults + breakfastChildren + breakfastBabies +
                    lunchAdults + lunchChildren + lunchBabies +
                    dinnerAdults + dinnerChildren + dinnerBabies == 0)
                {
                    continue; 
                }

                results.Add(new BookingManagementReportGuestCountSearchResultViewModel
                {
                    DayOfMonth = day,
                    BreakfastAdults = breakfastAdults,
                    BreakfastChildren = breakfastChildren,
                    BreakfastBabies = breakfastBabies,
                    LunchAdults = lunchAdults,
                    LunchChildren = lunchChildren,
                    LunchBabies = lunchBabies,
                    DinnerAdults = dinnerAdults,
                    DinnerChildren = dinnerChildren,
                    DinnerBabies = dinnerBabies
                });
            }

            return results;
        }

    }
}
