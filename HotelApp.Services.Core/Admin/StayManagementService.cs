namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Web.ViewModels.Admin.StayManagement.Search;
    using HotelApp.Data.Models;
    using HotelApp.Web.ViewModels;

    using HotelApp.Services.Common.Extensions;

    public class StayManagementService : IStayManagementService
    {
        private readonly IStayRepository stayRepository;
        private readonly IGuestRepository guestRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingManagementService bookingService;

        public StayManagementService(IStayRepository stayRepository,
            IGuestRepository guestRepository,
            IBookingRepository bookingRepository,
            IBookingManagementService bookingService)
        {
            this.stayRepository = stayRepository;
            this.guestRepository = guestRepository;
            this.bookingRepository = bookingRepository;
            this.bookingService = bookingService;
        }

        public async Task<IEnumerable<StayManagementIndexViewModel>> GetStayManagementBoardDataAsync()
        {
            return await stayRepository
                .GetAllAttached()
                .Include(s => s.Guest)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .OrderByDescending(s => s.CreatedOn)
                .Select(s => new StayManagementIndexViewModel
                {
                    Id = s.Id,
                    GuestNames = s.Guest.FirstName + " " + s.Guest.FamilyName,
                    CreatedOn = s.CreatedOn.ToHotelTime(),
                    IsDeleted = s.IsDeleted
                })
                .ToListAsync()
                ?? Enumerable.Empty<StayManagementIndexViewModel>();
        }

        public async Task AddStayManagementAsync(StayManagementCreateViewModel inputModel)
        {
            var guest = await this.guestRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(g => g.Email == inputModel.GuestEmail);

            if (guest == null)
            {
                throw new ArgumentException(ValidationMessages.Stay.GuestEmailNotFoundMessage);
            }

            bool guestAlreadyInBooking = await this.stayRepository
                .GetAllAttached()
                .AnyAsync(s => s.BookingId == inputModel.BookingId
                       && s.GuestId == guest.Id);

            if (guestAlreadyInBooking)
            {
                throw new InvalidOperationException(ValidationMessages.Stay.GuestEmailExistMessage);
            }

            var newStay = new Stay
            {
                BookingId = inputModel.BookingId,
                GuestId = guest.Id,
            };

            await this.stayRepository.AddAsync(newStay);

            var booking = await this.bookingService.FindBookingByIdAsync(inputModel.BookingId);

            if (booking != null && booking.StatusId == 3)
            {
                booking.StatusId = 4;  // In Progress
                await this.bookingRepository.SaveChangesAsync();
            }
        }

        public async Task<StayManagementDetailsViewModel?> GetStayManagementDetailsByIdAsync(string? id)
        {
            StayManagementDetailsViewModel? stayDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid stayId);

            if (isIdValidGuid)
            {
                stayDetails = await this.stayRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(s => s.Id == stayId)
                    .Select(s => new StayManagementDetailsViewModel()
                    {
                        Id = s.Id,
                        GuestId = s.Guest.Id,
                        GuestFirstName = s.Guest.FirstName,
                        GuestFamilyName = s.Guest.FamilyName,
                        GuestEmail = s.Guest.Email,
                        CreatedOn = s.CreatedOn.ToHotelTime(),
                        CheckoutOn = s.CheckoutOn.HasValue
                            ? s.CheckoutOn.Value.ToHotelTime()
                            : null,
                        IsDeleted = s.IsDeleted
                    })
                    .SingleOrDefaultAsync();
            }

            return stayDetails;
        }

        public async Task<StayManagementEditFormModel?> GetStayEditFormModelAsync(string? id)
        {
            StayManagementEditFormModel? formModel = null;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Stay? stayToEdit = await this.stayRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(s => s.Id.ToString().ToLower() == id.ToLower());
                if (stayToEdit != null)
                {
                    formModel = new StayManagementEditFormModel()
                    {
                        Id = stayToEdit.Id.ToString(),
                        CheckoutOn = stayToEdit.CheckoutOn,
                        BookingId = stayToEdit.BookingId,
                    };
                }
            }

            return formModel;
        }

        public async Task<bool> EditStayAsync(StayManagementEditFormModel? inputModel)
        {
            if (inputModel == null) 
            {
                return false;
            }

            var stayToEdit = await this.stayRepository
                .GetAllAttached()
                .SingleOrDefaultAsync(s => s.Id.ToString().ToLower() == inputModel.Id.ToLower());

            if (stayToEdit == null)
            {
                return false;
            }

            stayToEdit.CheckoutOn = DateTime.UtcNow;
            var stayUpdated = await this.stayRepository.UpdateAsync(stayToEdit);

            if (!stayUpdated)
            {
                return false;
            }

            var booking = await this.bookingService.FindBookingByIdAsync(inputModel.BookingId);

            if (booking != null)
            {
                var allStays = await this.stayRepository
                    .GetAllAttached()
                    .Where(s => s.BookingId == booking.Id && !s.IsDeleted)
                    .ToListAsync();

                var remainingStays = allStays.Where(s => s.CheckoutOn == null).ToList();
                var expectedGuests = booking.AdultsCount + booking.ChildCount + booking.BabyCount;

                if (!remainingStays.Any() && allStays.Count == expectedGuests)
                {
                    var lastCheckout = allStays.Max(s => s.CheckoutOn!.Value);
                    var lastCheckoutDate = DateOnly.FromDateTime(lastCheckout);

                    booking.StatusId = lastCheckoutDate < booking.DateDeparture
                        ? 6 // Done - Early Check Out
                        : 5; // Done

                    await this.bookingRepository.UpdateAsync(booking);
                }
            }

            return true;
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreStayAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Stay? stay = await this.stayRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(s => s.Id.ToString().ToLower() == id.ToLower());
                if (stay != null)
                {
                    if (stay.IsDeleted)
                    {
                        isRestored = true;
                    }

                    stay.IsDeleted = !stay.IsDeleted;

                    result = await this.stayRepository
                        .UpdateAsync(stay);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

        public async Task<GuestAgeStatsViewModel> GetGuestAgeStatsAsync()
        {
            var stays = await this.stayRepository
                .GetAllAttached()
                .Include(s => s.Guest)
                .Where(s =>
                    !s.IsDeleted &&
                    s.CheckoutOn == null && 
                    s.Guest != null &&
                    !s.Guest.IsDeleted &&
                    s.Guest.BirthDate.HasValue
                )
                .ToListAsync();

            var uniqueGuests = stays
                .GroupBy(s => s.Guest!.Id)
                .Select(g => g.First().Guest!)
                .ToList();

            var stats = new GuestAgeStatsViewModel
            {
                Babies = uniqueGuests.Count(g => g.Age <= 3),
                Children = uniqueGuests.Count(g => g.Age >= 4 && g.Age <= 17),
                Adults = uniqueGuests.Count(g => g.Age >= 18)
            };

            return stats;
        }

        public async Task<MealGuestAgeStatsViewModel> GetMealGuestAgeStatsAsync()
        {
            DateTime nowLocal = DateTime.UtcNow.ToHotelTime();
            DateOnly today = DateOnly.FromDateTime(nowLocal);

            DateTime breakfastStart = today.ToDateTime(new TimeOnly(7, 30));
            DateTime lunchStart = today.ToDateTime(new TimeOnly(12, 30));
            DateTime dinnerStart = today.ToDateTime(new TimeOnly(18, 0));

            var stays = await this.stayRepository
                .GetAllAttached()
                .Include(s => s.Guest)
                .Include(s => s.Booking)
                .Where(s =>
                    !s.IsDeleted &&
                    s.Guest != null &&
                    !s.Guest.IsDeleted &&
                    s.Guest.BirthDate.HasValue
                )
                .ToListAsync();

            var statsMeal = new MealGuestAgeStatsViewModel();

            var breakfastGuests = stays
                .Where(s =>
                {
                    DateTime? checkoutLocal = s.CheckoutOn.ToHotelTime();

                    bool stillStaying =
                        s.Booking.DateArrival < today &&
                        s.Booking.DateDeparture > today &&
                        checkoutLocal == null;

                    bool departureTodayAndStillStaying =
                        s.Booking.DateDeparture == today &&
                        checkoutLocal == null;

                    bool departureTodayAndCheckoutAfterBreakfastStart =
                        s.Booking.DateDeparture == today &&
                        checkoutLocal != null &&
                        checkoutLocal >= breakfastStart;

                    return
                        stillStaying ||
                        departureTodayAndStillStaying ||
                        departureTodayAndCheckoutAfterBreakfastStart;
                })
                .GroupBy(s => s.Guest!.Id)
                .Select(g => g.First().Guest!)
                .ToList();

            statsMeal.BreakfastBabies = breakfastGuests.Count(g => g.Age <= 3);
            statsMeal.BreakfastChildren = breakfastGuests.Count(g => g.Age >= 4 && g.Age <= 17);
            statsMeal.BreakfastAdults = breakfastGuests.Count(g => g.Age >= 18);

            var lunchGuests = stays
                .Where(s =>
                {
                    DateTime? checkoutLocal = s.CheckoutOn.ToHotelTime();

                    bool stillStaying =
                        s.Booking.DateArrival < today &&
                        s.Booking.DateDeparture > today &&
                        checkoutLocal == null;

                    bool checkoutAfterLunchStart =
                        s.Booking.DateArrival < today &&
                        s.Booking.DateDeparture > today &&
                        checkoutLocal != null &&
                        checkoutLocal >= lunchStart;

                    return
                        stillStaying ||
                        checkoutAfterLunchStart;
                })
                .GroupBy(s => s.Guest!.Id)
                .Select(g => g.First().Guest!)
                .ToList();

            statsMeal.LunchBabies = lunchGuests.Count(g => g.Age <= 3);
            statsMeal.LunchChildren = lunchGuests.Count(g => g.Age >= 4 && g.Age <= 17);
            statsMeal.LunchAdults = lunchGuests.Count(g => g.Age >= 18);

            var dinnerGuests = stays
                .Where(s =>
                {
                    DateTime? checkoutLocal = s.CheckoutOn.ToHotelTime();

                    bool stillStaying =
                        s.Booking.DateArrival < today &&
                        s.Booking.DateDeparture > today &&
                        checkoutLocal == null;

                    bool arrivedTodayAndStillStaying =
                        s.Booking.DateArrival == today &&
                        checkoutLocal == null;

                    bool arrivedTodayAndCheckoutAfterDinnerStart =
                        s.Booking.DateArrival == today &&
                        checkoutLocal != null &&
                        checkoutLocal >= dinnerStart;

                    return
                        stillStaying ||
                        arrivedTodayAndStillStaying ||
                        arrivedTodayAndCheckoutAfterDinnerStart;
                })
                .GroupBy(s => s.Guest!.Id)
                .Select(g => g.First().Guest!)
                .ToList();

            statsMeal.DinnerBabies = dinnerGuests.Count(g => g.Age <= 3);
            statsMeal.DinnerChildren = dinnerGuests.Count(g => g.Age >= 4 && g.Age <= 17);
            statsMeal.DinnerAdults = dinnerGuests.Count(g => g.Age >= 18);

            return statsMeal;
        }

        public async Task<IEnumerable<StayManagementSearchResultViewModel>> SearchStayAsync(StayManagementSearchInputModel inputModel)
        {
            var query = stayRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AsQueryable();

            // Stay Id
            if (!string.IsNullOrWhiteSpace(inputModel.Id))
            {
                if (!Guid.TryParse(inputModel.Id, out Guid stayId))
                {
                    return new List<StayManagementSearchResultViewModel>();
                }
                query = query.Where(s => s.Id == stayId);
            }

            // Booking Id
            if (!string.IsNullOrWhiteSpace(inputModel.BookingId))
            {
                if (!Guid.TryParse(inputModel.BookingId, out Guid bookingId))
                {
                    return new List<StayManagementSearchResultViewModel>();
                }
                query = query.Where(s => s.BookingId == bookingId);
            }

            // Guest Id
            if (!string.IsNullOrWhiteSpace(inputModel.GuestId))
            {
                if (!Guid.TryParse(inputModel.GuestId, out Guid guestId))
                {
                    return new List<StayManagementSearchResultViewModel>();
                }
                query = query.Where(s => s.GuestId == guestId);
            }

            // Created On
            if (inputModel.CreatedOn.HasValue)
            {
                var createdDate = inputModel.CreatedOn.Value.Date;

                query = query.Where(s =>
                    s.CreatedOn.Date == createdDate);
            }

            if (inputModel.CheckoutOn.HasValue)
            {
                DateTime checkoutDate = inputModel.CheckoutOn.Value.Date;

                query = query.Where(s =>
                    s.CheckoutOn.HasValue &&
                    s.CheckoutOn.Value.Date == checkoutDate);
            }

            // IsDeleted
            if (inputModel.IsDeleted.HasValue)
            {
                query = query.Where(s =>
                    s.IsDeleted == inputModel.IsDeleted.Value);
            }

            var stays = await query
                .OrderByDescending(s => s.CreatedOn)
                .Select(s => new StayManagementSearchResultViewModel
                {
                    Id = s.Id.ToString(),
                    CreatedOn = s.CreatedOn,
                    BookingId = s.BookingId.ToString(),
                    CheckoutOn = s.CheckoutOn.HasValue ? s.CheckoutOn.Value.ToString("yyyy-MM-dd") : "-",
                    IsDeleted = s.IsDeleted
                })
                .ToListAsync();

            return stays;
        }

    }
}
