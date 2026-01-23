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

            // 1️⃣ Зареждаме всички стаи с категории
            var roomsList = await roomRepository
                .GetAllAttached()
                .Include(r => r.Category)
                .AsNoTracking()
                .ToListAsync();

            // 2️⃣ Зареждаме всички активни резервации с BookingRooms
            var bookings = await bookingRepository.GetAllAttached()
                .Where(b => !b.IsDeleted && b.StatusId != 2) // Cancelled
                .Include(b => b.BookingRooms)
                .ToListAsync();

            // 3️⃣ Зареждаме stays с CheckoutOn
            var stays = await stayRepository.GetAllAttached()
                .Where(s => !s.IsDeleted && s.CheckoutOn.HasValue)
                .ToListAsync();

            // 4️⃣ Филтрираме свободните стаи
            var freeRooms = roomsList
                .Where(r =>
                {
                    var roomBookings = bookings
                        .Where(b => b.BookingRooms.Any(br => br.RoomId == r.Id))
                        .ToList();

                    // Ако стаята никога не е била резервирана → свободна
                    if (!roomBookings.Any())
                        return true;

                    // Намираме последната дата на заетост за стаята
                    DateOnly? lastOccupiedDate = roomBookings
                        .Select(b =>
                        {
                            // Вземаме BookingRoom(s) за стаята
                            var brDates = b.BookingRooms
                                .Where(br => br.RoomId == r.Id)
                                .Select(br =>
                                {
                                    // Ако е Done - Early Check Out → взимаме Max Checkout от stay
                                    if (br.StatusId == 6)
                                    {
                                        var maxStay = stays
                                            .Where(s => s.BookingRoomId == br.Id)
                                            .Select(s => (DateOnly?)DateOnly.FromDateTime(s.CheckoutOn.Value))
                                            .DefaultIfEmpty(null)
                                            .Max();

                                        return maxStay;
                                    }

                                    // Иначе използваме DateDeparture на резервацията
                                    return b.DateDeparture;
                                })
                                .DefaultIfEmpty(null)
                                .Max();

                            return brDates;
                        })
                        .DefaultIfEmpty(null)
                        .Max();

                    // Стаята е свободна, ако checkin >= последната заетост
                    return !lastOccupiedDate.HasValue || checkin >= lastOccupiedDate;
                })
                .ToList();

            // 5️⃣ Вземаме максимум 3 стаи от всяка категория
            var rooms = freeRooms
                .GroupBy(r => r.CategoryId)
                .SelectMany(g => g.OrderBy(r => r.Name).Take(3))
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

            var roomsList = await roomRepository
                .GetAllAttached()
                .Include(r => r.Category)
                .AsNoTracking()
                .Where(r => r.CategoryId == categoryId)
                .ToListAsync();

            var bookings = await bookingRepository.GetAllAttached()
                .Where(b => !b.IsDeleted && b.StatusId != 2) // Status Cancelled
                .Include(b => b.BookingRooms)
                .ToListAsync();

            var stays = await stayRepository.GetAllAttached()
                .Where(s => !s.IsDeleted && s.CheckoutOn.HasValue)
                .ToListAsync();

            var availableRooms = roomsList
                .Where(r =>
                {
                    var roomBookings = bookings
                        .Where(b => b.BookingRooms.Any(br => br.RoomId == r.Id))
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
                            )
                        )
                        .DefaultIfEmpty(null)
                        .Max();

                    return !lastOccupiedDate.HasValue || checkin >= lastOccupiedDate;
                })
                .OrderBy(r => r.Name)
                .Take(3)
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
