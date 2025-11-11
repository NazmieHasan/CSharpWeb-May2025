namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.RoomManagement;

    public interface IRoomManagementService
    {
        Task<IEnumerable<RoomManagementIndexViewModel>> GetRoomManagementBoardDataAsync();
    }
}
