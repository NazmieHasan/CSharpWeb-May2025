namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.StayManagement;

    public interface IStayManagementService
    {
        Task<IEnumerable<StayManagementIndexViewModel>> GetStayManagementBoardDataAsync();

        Task AddStayManagementAsync(StayManagementCreateViewModel inputModel);

        Task<StayManagementEditFormModel?> GetStayEditFormModelAsync(string? id);

        Task<bool> EditStayAsync(StayManagementEditFormModel? inputModel);
    }
}
