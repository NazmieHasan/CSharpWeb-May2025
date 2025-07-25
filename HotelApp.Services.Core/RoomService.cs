namespace HotelApp.Services.Core
{

    using Interfaces;
    using Data;
    using Microsoft.EntityFrameworkCore;

    using Web.ViewModels.Room;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RoomService : IRoomService
    {
        private readonly HotelAppDbContext dbContext;

        public RoomService(HotelAppDbContext dbContext)
        {
            this.dbContext = dbContext;
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
    }
}
