namespace HotelApp.Services.Core
{

    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Interfaces;
    using Web.ViewModels.Category;
    using HotelApp.Web.ViewModels.Room;

    public class RoomService : IRoomService
    {
        private readonly HotelAppDbContext dbContext;

        public RoomService(HotelAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddRoomAsync(AddRoomInputModel inputModel)
        {
            bool opRes = false;

            Category? catRef = await this.dbContext
                .Categories
                .FindAsync(inputModel.CategoryId);

            if (catRef != null)
            {
                Room newRecipe = new Room()
                {
                    Name = inputModel.Name,
                    CategoryId = inputModel.CategoryId
                };

                await this.dbContext.Rooms.AddAsync(newRecipe);
                await this.dbContext.SaveChangesAsync();

                opRes = true;
            }

            return opRes;
        }

        public async Task<IEnumerable<AllRoomsIndexViewModel>> GetAllRoomsAsync()
        {
            IEnumerable<AllRoomsIndexViewModel> allRooms = await this.dbContext
                .Rooms
                .Include(r => r.Category)
                .Where(r => !r.IsDeleted)
                .Select(r => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    Category = r.Category.Name,
                    CategoryId = r.CategoryId,
                })
                .ToArrayAsync();

            return allRooms;
        }

        public async Task<RoomDetailsViewModel?> GetRoomDetailsByIdAsync(string? id)
        {
            RoomDetailsViewModel? roomDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid roomId);

            if (isIdValidGuid)
            {
                roomDetails = await this.dbContext
                    .Rooms
                    .Include(r => r.Category)
                    .AsNoTracking()
                    .Where(r => r.Id == roomId)
                    .Select(r => new RoomDetailsViewModel()
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        Category = r.Category.Name
                    })
                    .SingleOrDefaultAsync();
            }

            return roomDetails;
        }
    }
}
