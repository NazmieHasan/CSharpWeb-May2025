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
                   ManagerName = b.Manager != null ?
                        b.Manager.User.UserName : null,
                   IsDeleted = b.IsDeleted ? "Yes" : "No"
               })
               .ToListAsync()
               ?? Enumerable.Empty<BookingManagementIndexViewModel>();
        }
    }
}
