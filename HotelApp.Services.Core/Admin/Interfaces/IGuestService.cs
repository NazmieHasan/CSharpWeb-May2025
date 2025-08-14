namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.GuestManagement;

    public interface IGuestService
    {
        Task<IEnumerable<GuestManagementIndexViewModel>> GetGuestManagementBoardDataAsync();
    }
}
