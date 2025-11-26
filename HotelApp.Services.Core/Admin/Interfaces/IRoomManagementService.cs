namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.RoomManagement;

    public interface IRoomManagementService
    {
        Task<IEnumerable<RoomManagementIndexViewModel>> GetRoomManagementBoardDataAsync();

        Task<bool> AddRoomManagementAsync(AddRoomManagementInputModel inputModel);

        Task<RoomManagementDetailsViewModel?> GetRoomDetailsByIdAsync(string? id);

        Task<EditRoomManagementInputModel?> GetRoomForEditAsync(string? id);

        Task<bool> PersistUpdatedRoomAsync(EditRoomManagementInputModel inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreRoomAsync(string? id);
    }
}
