namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.CategoryManagement;
    using HotelApp.Web.ViewModels.Admin.RoomManagement;

    public interface ICategoryManagementService
    {
        Task<IEnumerable<CategoryManagementIndexViewModel>> GetCategoryManagementBoardDataAsync();

        Task AddCategoryManagementAsync(CategoryManagementFormInputModel inputModel);

        Task<CategoryManagementDetailsViewModel?> GetCategoryDetailsByIdAsync(int? id);

        Task<CategoryManagementFormInputModel?> GetEditableCategoryByIdAsync(int? id);

        Task<bool> EditCategoryAsync(CategoryManagementFormInputModel inputModel);

        Task<IEnumerable<AddRoomCategoryDropDownModel>> GetCategoriesDropDownDataAsync();

        Task<Tuple<bool, bool>> DeleteOrRestoreCategoryAsync(int? id);
    }
}
