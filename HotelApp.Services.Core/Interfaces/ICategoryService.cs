namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Category;
    using HotelApp.Web.ViewModels.Room;

    public interface ICategoryService
    {
        Task<IEnumerable<AllCategoriesIndexViewModel>> GetAllCategoriesAsync();

        Task AddCategoryAsync(CategoryFormInputModel inputModel);

        Task<CategoryDetailsViewModel?> GetCategoryDetailsByIdAsync(int? id);

        Task<CategoryFormInputModel?> GetEditableCategoryByIdAsync(int? id);

        Task<bool> EditCategoryAsync(CategoryFormInputModel inputModel);

        Task<bool> SoftDeleteCategoryAsync(int? id);

        Task<bool> DeleteCategoryAsync(int? id);

        Task<DeleteCategoryViewModel?> GetCategoryDeleteDetailsByIdAsync(int? id);

        Task<IEnumerable<AddRoomCategoryDropDownModel>> GetCategoriesDropDownDataAsync();
    }
}
