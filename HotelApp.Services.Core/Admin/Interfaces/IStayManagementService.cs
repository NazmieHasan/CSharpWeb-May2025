namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.StayManagement;

    public interface IStayManagementService
    {
        Task<IEnumerable<StayManagementIndexViewModel>> GetStayManagementBoardDataAsync();

        Task AddStayManagementAsync(StayManagementCreateViewModel inputModel);

        Task<StayManagementDetailsViewModel?> GetStayManagementDetailsByIdAsync(string? id);

        Task<StayManagementEditFormModel?> GetStayEditFormModelAsync(string? id);

        Task<bool> EditStayAsync(StayManagementEditFormModel? inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreStayAsync(string? id);

        Task<GuestAgeStatsViewModel> GetGuestAgeStatsAsync();

        Task<MealGuestAgeStatsViewModel> GetMealGuestAgeStatsAsync();
    }
}
