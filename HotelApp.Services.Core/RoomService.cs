namespace HotelApp.Services.Core
{

    using System.Globalization;

    using Microsoft.EntityFrameworkCore;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Room;
    using static GCommon.ApplicationConstants;
    using System.Collections.Generic;
    using HotelApp.Data.Repository;
    using HotelApp.GCommon;

    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IStayRepository stayRepository;

        public RoomService(IRoomRepository roomRepository,
            IBookingRepository bookingRepository,
            IStayRepository stayRepository)
        {
            this.roomRepository = roomRepository;
            this.bookingRepository = bookingRepository;
            this.stayRepository = stayRepository;
        }
        public async Task<RoomDetailsViewModel?> GetRoomDetailsByIdAsync(string? id)
        {
            RoomDetailsViewModel? roomDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid roomId);

            if (isIdValidGuid)
            {
                roomDetails = await this.roomRepository
                    .GetAllAttached()
                    .Include(r => r.Category)
                    .AsNoTracking()
                    .Where(r => r.Id == roomId)
                    .Select(r => new RoomDetailsViewModel()
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        CategoryName = r.Category.Name,
                        CategoryPrice = r.Category.Price,
                        CategoryBeds = r.Category.Beds
                    })
                    .SingleOrDefaultAsync();
            }

            return roomDetails;
        }

        public async Task<IEnumerable<AllRoomsIndexViewModel>> FindRoomByDateArrivaleAndDateDepartureAsync(FindRoomInputModel inputModel)
        {
            var checkin = inputModel.DateArrival;
            var checkout = inputModel.DateDeparture;

            // 1️⃣ Load all rooms with categories
            var roomsList = await roomRepository
                .GetAllAttached()
                .Include(r => r.Category)
                .AsNoTracking()
                .ToListAsync();

            // 2️⃣ Load all active bookings with BookingRooms
            var bookings = await bookingRepository.GetAllAttached()
                .Where(b => !b.IsDeleted && b.StatusId != 2) // Cancelled
                .Include(b => b.BookingRooms)
                .ToListAsync();

            // 3️⃣ Load stays with CheckoutOn
            var stays = await stayRepository.GetAllAttached()
                .Where(s => !s.IsDeleted && s.CheckoutOn.HasValue)
                .ToListAsync();

            // 4️⃣ Filter free rooms
            var freeRooms = roomsList
                .Where(r =>
                {
                    var roomBookings = bookings
                        .Where(b => b.BookingRooms.Any(br => br.RoomId == r.Id))
                        .ToList();

                    // If the room has never been booked → free
                    if (!roomBookings.Any())
                        return true;

                    // Find the last occupancy date for the room
                    DateOnly? lastOccupiedDate = roomBookings
                        .Select(b =>
                        {
                            // Get BookingRooms for the room
                            var brDates = b.BookingRooms
                                .Where(br => br.RoomId == r.Id)
                                .Select(br =>
                                {
                                    // If Done - Early Check Out → take Max Checkout from stay
                                    if (br.StatusId == 6)
                                    {
                                        var maxStay = stays
                                            .Where(s => s.BookingRoomId == br.Id)
                                            .Select(s => (DateOnly?)DateOnly.FromDateTime(s.CheckoutOn.Value))
                                            .DefaultIfEmpty(null)
                                            .Max();

                                        return maxStay;
                                    }

                                    // Otherwise use the booking's DateDeparture
                                    return b.DateDeparture;
                                })
                                .DefaultIfEmpty(null)
                                .Max();

                            return brDates;
                        })
                        .DefaultIfEmpty(null)
                        .Max();

                    // The room is free if checkin >= last occupancy
                    return !lastOccupiedDate.HasValue || checkin >= lastOccupiedDate;
                })
                .ToList();

            // 5️⃣ Take a maximum of 3 rooms from each category
            var rooms = freeRooms
                .GroupBy(r => r.CategoryId)
                .SelectMany(g => g.OrderBy(r => r.Name).Take(ApplicationConstants.AllowedMaxCountRoomByCategoryForBooking))
                .OrderBy(r => r.Name)
                .Select(r => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    CategoryId = r.CategoryId,
                    Category = r.Category.Name,
                    ImageUrl = r.Category.ImageUrl,
                    CategoryBeds = r.Category.Beds,
                    CategoryPrice = r.Category.Price
                })
                .ToList();

            return rooms;
        }

        public async Task<IEnumerable<AllRoomsIndexViewModel>> FindRoomByDateArrivaleDateDepartureAndCategoryAsync(FindRoomInputModel inputModel)
        {
            var checkin = inputModel.DateArrival;
            var checkout = inputModel.DateDeparture;
            var categoryId = inputModel.CategoryId;

            // 1️⃣ Load all rooms of the specified category including their category details
            var roomsList = await roomRepository
                .GetAllAttached()
                .Include(r => r.Category)
                .AsNoTracking()
                .Where(r => r.CategoryId == categoryId)
                .ToListAsync();

            // 2️⃣ Load all active bookings including their BookingRooms
            var bookings = await bookingRepository.GetAllAttached()
                .Where(b => !b.IsDeleted && b.StatusId != 2) // Status Cancelled
                .Include(b => b.BookingRooms)
                .ToListAsync();

            // 3️⃣ Load all stays with a CheckoutOn date
            var stays = await stayRepository.GetAllAttached()
                .Where(s => !s.IsDeleted && s.CheckoutOn.HasValue)
                .ToListAsync();

            // 4️⃣ Filter available rooms based on bookings and stays
            var availableRooms = roomsList
                .Where(r =>
                {
                    // Get all bookings that include this room
                    var roomBookings = bookings
                        .Where(b => b.BookingRooms.Any(br => br.RoomId == r.Id))
                        .ToList();

                    // If the room has never been booked, it's available
                    if (!roomBookings.Any())
                        return true;

                    // Determine the last occupancy date for the room
                    DateOnly? lastOccupiedDate = roomBookings
                        .SelectMany(b => b.BookingRooms
                            .Where(br => br.RoomId == r.Id)
                            .Select(br =>
                                br.StatusId == 6 // Status Done - Early Check Out
                                    ? stays
                                        .Where(s => s.BookingRoomId == br.Id)
                                        .Select(s => (DateOnly?)DateOnly.FromDateTime(s.CheckoutOn.Value))
                                        .DefaultIfEmpty(null)
                                        .Max()
                                    : b.DateDeparture
                            )
                        )
                        .DefaultIfEmpty(null)
                        .Max();

                    // Room is available if the requested check-in date is after the last occupied date
                    return !lastOccupiedDate.HasValue || checkin >= lastOccupiedDate;
                })
                .OrderBy(r => r.Name)
                .Take(ApplicationConstants.AllowedMaxCountRoomByCategoryForBooking)
                .Select(r => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    CategoryId = r.CategoryId,
                    Category = r.Category.Name,
                    ImageUrl = r.Category.ImageUrl,
                    CategoryBeds = r.Category.Beds,
                    CategoryPrice = r.Category.Price
                })
                .ToList();

            return availableRooms;
        }

    }
}
