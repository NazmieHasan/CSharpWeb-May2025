namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.ManagerManagement;

    public interface IManagerManagementService
    {
        Task<IEnumerable<ManagerManagementIndexViewModel>> GetManagerManagementBoardDataAsync();

        Task AddManagerManagementAsync(ManagerManagementCreateViewModel inputModel);
    }
}
