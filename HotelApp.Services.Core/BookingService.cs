namespace HotelApp.Services.Core
{
    using System.Globalization;

    using Microsoft.EntityFrameworkCore;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Booking;

    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using HotelApp.Web.ViewModels.Manager;

    using static GCommon.ApplicationConstants;
    using static HotelApp.Services.Core.DateTimeExtensions;
    using HotelApp.GCommon;

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IManagerRepository managerRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingService(IBookingRepository bookingRepository,
            IManagerRepository managerRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.bookingRepository = bookingRepository;
            this.managerRepository = managerRepository;
            this.userManager = userManager;
        }

        public async Task<bool> AddBookingAsync(string userId, AddBookingInputModel inputModel)
        {
            if (inputModel.DateArrival < DateOnly.FromDateTime(DateTime.UtcNow) ||
                inputModel.DateDeparture <= inputModel.DateArrival)
            {
                return false;
            }

            IdentityUser? user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var newBooking = new Booking
            {
                DateArrival = inputModel.DateArrival,
                DateDeparture = inputModel.DateDeparture,
                AdultsCount = inputModel.AdultsCount,
                ChildCount = inputModel.ChildCount,
                BabyCount = inputModel.BabyCount,
                UserId = userId,
                RoomId = inputModel.RoomId,
                Owner = inputModel.Owner,
                IsForAnotherPerson = inputModel.IsForAnotherPerson
            };

            await this.bookingRepository.AddAsync(newBooking);

            return true;
        }

        public async Task<IEnumerable<MyBookingsViewModel>> GetBookingsByUserIdAsync(string userId, int pageNumber = 1, int pageSize = ApplicationConstants.MyBookingsPaginationPageSize)  
        {
            var query = this.bookingRepository
                .GetAllAttached()
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Include(b => b.Status)
                .AsNoTracking()
                .Where(b => b.UserId.ToLower() == userId.ToLower())
                .OrderByDescending(b => b.CreatedOn)
                .Select(b => new MyBookingsViewModel()
                {
                    Id = b.Id.ToString(),
                    CreatedOn = b.CreatedOn.ToHotelTime(),
                    DateArrival = b.DateArrival,
                    DateDeparture = b.DateDeparture,
                    AdultsCount = b.AdultsCount,
                    ChildCount = b.ChildCount,
                    BabyCount = b.BabyCount,
                    TotalAmount = b.TotalAmount,
                    PaidAmount = b.Payments.Sum(p => p.Amount),
                    RemainingAmount = b.TotalAmount - b.Payments.Sum(p => p.Amount),
                    Category = b.Room.Category.Name,
                    Status = b.Status.Name,
                });

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<int> GetBookingsCountByUserIdAsync(string userId)
        {
            return await this.bookingRepository
                .GetAllAttached()
                .CountAsync(b => b.UserId.ToLower() == userId.ToLower());
        }

        /* Manager method */
        public async Task<IEnumerable<ManagerBookingsIndexViewModel>> GetBookingsByManagerIdAsync(string userId)
        {
            var manager = await this.managerRepository
                .GetAllAttached() 
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (manager == null)
            {
                return Enumerable.Empty<ManagerBookingsIndexViewModel>();
            }

            var managerGuid = manager.Id;

            return await this.bookingRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(b => b.User)
                .Include(b => b.Manager)
                .Where(b => b.ManagerId == managerGuid)
                .OrderByDescending(b => b.CreatedOn)
                .Select(b => new ManagerBookingsIndexViewModel()
                {
                    Id = b.Id.ToString(),
                    CreatedOn = b.CreatedOn,
                    UserEmail = b.User.Email,
                    ManagerEmail = b.Manager != null ? b.Manager.User.UserName : null,
                    IsDeleted = b.IsDeleted
                })
                .ToListAsync();
        }

        /* Manager method */
        public async Task<ManagerBookingDetailsViewModel?> GetBookingDetailsByIdAsync(string? id)
        {
            ManagerBookingDetailsViewModel? bookingDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid bookingId);

            if (isIdValidGuid)
            {
                bookingDetails = await this.bookingRepository
                    .GetAllAttached()
                    .Include(b => b.Room)
                    .AsNoTracking()
                    .Where(b => b.Id == bookingId)
                    .Select(b => new ManagerBookingDetailsViewModel()
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

        /* Booking API method */
        public async Task<IEnumerable<string>> GetBookingsIdByUserIdAsync(string? userId)
        {
            IEnumerable<string> bookingIds = new List<string>();
            if (!String.IsNullOrWhiteSpace(userId))
            {
                bookingIds = await this.bookingRepository
                    .GetAllAttached()
                    .Where(b => b.UserId.ToString().ToLower() == userId.ToLower())
                    .OrderByDescending(b => b.CreatedOn)
                    .Select(b => b.Id.ToString())
                    .ToArrayAsync();
            }

            return bookingIds;
        }

        /* Booking API method */
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

    }
}
