namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.ManagerManagement;

    public interface IManagerManagementService
    {
        Task<IEnumerable<ManagerManagementIndexViewModel>> GetManagerManagementBoardDataAsync();

        Task AddManagerManagementAsync(ManagerManagementCreateViewModel inputModel);

        Task<ManagerManagementDetailsViewModel?> GetManagerManagementDetailsByIdAsync(string? id);

        Task<Tuple<bool, bool>> DeleteOrRestoreManagerAsync(string? id);
    }
}
