namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.GuestManagement;

    public interface IGuestManagementService
    {
        Task<IEnumerable<GuestManagementIndexViewModel>> GetGuestManagementBoardDataAsync();

        Task AddGuestManagementAsync(GuestManagementCreateViewModel inputModel);
    }
}
