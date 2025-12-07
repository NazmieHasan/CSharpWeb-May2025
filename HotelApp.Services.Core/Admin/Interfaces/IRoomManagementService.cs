namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Admin.RoomManagement;

    public interface IRoomManagementService
    {
        Task<IEnumerable<RoomManagementIndexViewModel>> GetRoomManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize);

        Task<int> GetTotalRoomsCountAsync();

        Task<bool> AddRoomManagementAsync(AddRoomManagementInputModel inputModel);

        Task<RoomManagementDetailsViewModel?> GetRoomDetailsByIdAsync(string? id);

        Task<EditRoomManagementInputModel?> GetRoomForEditAsync(string? id);

        Task<bool> PersistUpdatedRoomAsync(EditRoomManagementInputModel inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreRoomAsync(string? id);
    }
}
