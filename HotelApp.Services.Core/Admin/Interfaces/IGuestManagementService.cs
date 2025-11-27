namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.GuestManagement;

    public interface IGuestManagementService
    {
        Task<IEnumerable<GuestManagementIndexViewModel>> GetGuestManagementBoardDataAsync();

        Task AddGuestManagementAsync(GuestManagementCreateViewModel inputModel);

        Task<GuestManagementDetailsViewModel?> GetGuestManagementDetailsByIdAsync(string? id);

        Task<GuestManagementEditViewModel?> GetGuestEditFormModelAsync(string? id);

        Task<bool> EditGuestAsync(GuestManagementEditViewModel? inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreGuestAsync(string? id);
    }
}
