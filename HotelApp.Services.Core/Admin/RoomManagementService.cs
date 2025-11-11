namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.RoomManagement;

    public class RoomManagementService : IRoomManagementService
    {
        private readonly IRoomRepository roomRepository;

        public RoomManagementService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        } 

        public async Task<IEnumerable<RoomManagementIndexViewModel>> GetRoomManagementBoardDataAsync()
        {
            return await roomRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(r => new RoomManagementIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                })
                .ToListAsync()
                ?? Enumerable.Empty<RoomManagementIndexViewModel>();
        }
    }
}
