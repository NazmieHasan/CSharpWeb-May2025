namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Category;

    public interface ICategoryService
    {
        Task<IEnumerable<AllCategoriesIndexViewModel>> GetAllCategoriesAsync();

        Task AddCategoryAsync(CategoryFormInputModel inputModel);

        Task<CategoryDetailsViewModel?> GetCategoryDetailsByIdAsync(int? id);
    }
}
