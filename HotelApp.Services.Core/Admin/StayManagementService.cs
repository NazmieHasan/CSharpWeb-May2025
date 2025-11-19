namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Data.Models;
    using HotelApp.Web.ViewModels;

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
                .AsNoTracking()
                .Select(s => new StayManagementIndexViewModel
                {
                    Id = s.Id,
                    CreatedOn = s.CreatedOn,
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

    }
}
