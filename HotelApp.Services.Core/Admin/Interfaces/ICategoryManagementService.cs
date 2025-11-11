namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.CategoryManagement; 

    public interface ICategoryManagementService
    {
        Task<IEnumerable<CategoryManagementIndexViewModel>> GetCategoryManagementBoardDataAsync();
    }
}
