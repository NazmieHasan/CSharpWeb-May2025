namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.StatusManagement;

    public interface IStatusManagementService
    {
        Task<IEnumerable<StatusManagementIndexViewModel>> GetStatusManagementBoardDataAsync();
    }
}
