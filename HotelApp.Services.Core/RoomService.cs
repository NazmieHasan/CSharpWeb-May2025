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

        public RoomService(IRoomRepository roomRepository,
            IBookingRepository bookingRepository)
        {
            this.roomRepository = roomRepository;
            this.bookingRepository = bookingRepository;
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
                        Category = r.Category.Name,
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

            var rooms = await this.roomRepository
                .GetAllAttached()
                .Include(r => r.Category)        
                .AsNoTracking()
                .Where(r => !this.bookingRepository
                    .GetAllAttached()
                    .Any(b =>
                        b.RoomId == r.Id &&
                        b.DateArrival < checkout &&
                        b.DateDeparture > checkin))
                .Select(r => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    CategoryId = r.CategoryId,
                    Category = r.Category.Name,
                    ImageUrl = r.Category.ImageUrl
                })
                .ToListAsync();

            return rooms;
        }

        public async Task<AllRoomsIndexViewModel?> FindRoomByDateArrivaleDateDepartureAndCategoryAsync(FindRoomInputModel inputModel)
        {
            var checkin = inputModel.DateArrival;
            var checkout = inputModel.DateDeparture;

            var room = await this.roomRepository
                .GetAllAttached()
                .Include(r => r.Category)
                .AsNoTracking()
                .Where(r => r.CategoryId == inputModel.CategoryId)
                .Where(r => !this.bookingRepository.GetAllAttached()
                    .Any(b =>
                        b.RoomId == r.Id &&
                        b.DateArrival < checkout &&
                        b.DateDeparture > checkin))
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
