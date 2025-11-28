namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Data.Models;
    using HotelApp.Web.ViewModels;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

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
                .IgnoreQueryFilters()
                .AsNoTracking()
                .OrderByDescending(s => s.CreatedOn)
                .Select(s => new StayManagementIndexViewModel
                {
                    Id = s.Id,
                    CreatedOn = s.CreatedOn,
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
                        GuestEmail = s.Guest.Email,
                        CreatedOn = s.CreatedOn,
                        CheckoutOn = s.CheckoutOn,
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
    }
}
