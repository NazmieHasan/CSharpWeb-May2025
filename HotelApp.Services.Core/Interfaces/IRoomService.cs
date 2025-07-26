namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Room;

    public interface IRoomService
    {
        Task<IEnumerable<AllRoomsIndexViewModel>> GetAllRoomsAsync();

        Task<bool> AddRoomAsync(AddRoomInputModel inputModel);

        Task<RoomDetailsViewModel?> GetRoomDetailsByIdAsync(string? id);
    }
}
