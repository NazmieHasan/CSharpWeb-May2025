namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;

    public interface IRoomService
    {
        Task<IEnumerable<AllRoomsIndexViewModel>> GetAllRoomsAsync();

        Task<bool> AddRoomAsync(AddRoomInputModel inputModel);

        Task<RoomDetailsViewModel?> GetRoomDetailsByIdAsync(string? id);

        Task<EditRoomInputModel?> GetRoomForEditAsync(string? id);

        Task<bool> PersistUpdatedRoomAsync(EditRoomInputModel inputModel);

        Task<bool> SoftDeleteRoomAsync(string? id);

        Task<bool> DeleteRoomAsync(string? id);

        Task<DeleteRoomViewModel?> GetRoomDeleteDetailsByIdAsync(string? id);

        Task<IEnumerable<AllRoomsIndexViewModel>> FindRoomByDateArrivaleAndDateDepartureAsync(FindRoomInputModel inputModel);

        Task<AllRoomsIndexViewModel?> FindRoomByDateArrivaleDateDepartureAndCategoryAsync(FindRoomInputModel inputModel);
    }
}
