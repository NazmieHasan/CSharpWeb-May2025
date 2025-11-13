namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.StayManagement;

    public interface IStayManagementService
    {
        Task<IEnumerable<StayManagementIndexViewModel>> GetStayManagementBoardDataAsync();
    }
}
