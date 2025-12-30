namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Admin.GuestManagement;
    using HotelApp.Web.ViewModels.Admin.GuestManagement.Search;

    public interface IGuestManagementService
    {
        Task<IEnumerable<GuestManagementIndexViewModel>> GetGuestManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize);

        Task<int> GetTotalGuestsCountAsync();

        Task AddGuestManagementAsync(GuestManagementCreateViewModel inputModel);

        Task<GuestManagementDetailsViewModel?> GetGuestManagementDetailsByIdAsync(string? id);

        Task<GuestManagementEditViewModel?> GetGuestEditFormModelAsync(string? id);

        Task<bool> EditGuestAsync(GuestManagementEditViewModel? inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreGuestAsync(string? id);

        Task<IEnumerable<GuestManagementSearchResultViewModel>> SearchGuestAsync(GuestManagementSearchInputModel inputModel);
    }
}
