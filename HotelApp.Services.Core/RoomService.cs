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
                .Where(b => !b.IsDeleted && b.StatusId != 2) // Status Cancelled
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
                        .Where(b =>
                            b.BookingRooms.Any(br => br.RoomId == r.Id) &&
                            !(b.DateDeparture <= checkin || b.DateArrival >= checkout))
                        .ToList();

                    if (!roomBookings.Any())
                        return true;

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
                            ))
                        .DefaultIfEmpty(null)
                        .Max();

                    return !lastOccupiedDate.HasValue || checkin >= lastOccupiedDate;
                })
                .ToList();

            // 5️⃣ Group by category, calculate FreeRoomCountByCategory
            var groupedRooms = freeRooms
                .GroupBy(r => r.CategoryId)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    Rooms = g.OrderBy(r => r.Name) 
                             .Take(ApplicationConstants.AllowedMaxCountRoomByCategoryForBooking)
                             .ToList(),
                    FreeRoomCountByCategory = g.Count()
                })
                .ToList();

            var rooms = groupedRooms
                .SelectMany((g, catIndex) => g.Rooms
                    .OrderBy(r => r.Name) 
                    .Select((r, roomIndex) => new AllRoomsIndexViewModel
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        CategoryId = r.CategoryId,
                        Category = r.Category.Name,
                        ImageUrl = r.Category.ImageUrl,
                        CategoryBeds = r.Category.Beds,
                        CategoryPrice = r.Category.Price,
                        FreeRoomCountByCategory = g.FreeRoomCountByCategory,
                        RoomIndex = roomIndex,
                        CategoryIndex = catIndex
                    })
                )
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
            var freeRooms = roomsList
                .Where(r =>
                {
                    var roomBookings = bookings
                        .Where(b =>
                            b.BookingRooms.Any(br => br.RoomId == r.Id) &&
                            !(b.DateDeparture <= checkin || b.DateArrival >= checkout))
                        .ToList();

                    if (!roomBookings.Any())
                        return true;

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
                            ))
                        .DefaultIfEmpty(null)
                        .Max();

                    return !lastOccupiedDate.HasValue || checkin >= lastOccupiedDate;
                })
                .OrderBy(r => r.Name)
                .Take(ApplicationConstants.AllowedMaxCountRoomByCategoryForBooking)
                .ToList();

            // 5️⃣ Flatten list and set FreeRoomCountByCategory
            var rooms = freeRooms
                .OrderBy(r => r.Name)   
                .Select((r, index) => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    CategoryId = r.CategoryId,
                    Category = r.Category.Name,
                    ImageUrl = r.Category.ImageUrl,
                    CategoryBeds = r.Category.Beds,
                    CategoryPrice = r.Category.Price,
                    FreeRoomCountByCategory = freeRooms.Count, 
                    RoomIndex = index,
                    CategoryIndex = 0
                })
                .ToList();

            return rooms;
        }

    }
}
