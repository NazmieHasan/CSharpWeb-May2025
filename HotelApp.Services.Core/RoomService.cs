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

            var freeRoomsQuery =
                roomRepository
                    .GetAllAttached()
                    .Include(r => r.Category)
                    .AsNoTracking()
                    .Where(r =>
                        !bookingRepository
                            .GetAllAttached()
                            .Where(b =>
                                !b.IsDeleted &&
                                b.StatusId != 2 && // Status Cancelled
                                b.BookingRooms.Any(br =>
                                    br.RoomId == r.Id &&
                                    (
                                        br.StatusId != 6 || // Status Done - Early Check Out
                                        stayRepository.GetAllAttached()
                                            .Where(s => s.BookingRoomId == br.Id && !s.IsDeleted && s.CheckoutOn.HasValue)
                                            .Max(s => (DateOnly?)DateOnly.FromDateTime(s.CheckoutOn!.Value))
                                            > checkin
                                    )
                                )
                            )
                            .Any(b => b.DateArrival < checkout)
                    );

            var rooms = freeRoomsQuery
                .AsEnumerable()
                .GroupBy(r => r.CategoryId)
                .Select(g => g.OrderBy(r => r.Name).First())
                .OrderBy(r => r.Name)
                .Select(r => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    CategoryId = r.CategoryId,
                    Category = r.Category.Name,
                    ImageUrl = r.Category.ImageUrl
                })
                .ToList();

            return rooms;
        }

        public async Task<AllRoomsIndexViewModel?> FindRoomByDateArrivaleDateDepartureAndCategoryAsync(FindRoomInputModel inputModel)
        {
            var checkin = inputModel.DateArrival;
            var checkout = inputModel.DateDeparture;
            var categoryId = inputModel.CategoryId;

            var room = await roomRepository
                .GetAllAttached()
                .Include(r => r.Category)
                .AsNoTracking()
                .Where(r => r.CategoryId == categoryId)
                .Where(r =>
                    !bookingRepository
                        .GetAllAttached()
                        .Where(b =>
                            !b.IsDeleted &&
                            b.StatusId != 2 && // Status: Cancelled
                            b.BookingRooms.Any(br =>
                                br.RoomId == r.Id &&
                                (
                                    br.StatusId != 6 || // Status Done - Early Check Out
                                    stayRepository.GetAllAttached()
                                        .Where(s => s.BookingRoomId == br.Id && !s.IsDeleted && s.CheckoutOn.HasValue)
                                        .Max(s => (DateOnly?)DateOnly.FromDateTime(s.CheckoutOn!.Value))
                                        > checkin
                                )
                            )
                        )
                        .Any(b => b.DateArrival < checkout)
                )
                .OrderBy(r => r.Name)
                .Select(r => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    CategoryId = r.CategoryId,
                    Category = r.Category.Name,
                    ImageUrl = r.Category.ImageUrl
                })
                .FirstOrDefaultAsync();

            return room;
        }

    }
}
