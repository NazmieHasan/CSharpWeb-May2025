namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Category;
    using HotelApp.Web.ViewModels.Room;

    public interface ICategoryService
    {
        Task<IEnumerable<AllCategoriesIndexViewModel>> GetAllCategoriesAsync();

        Task<string?> FindCategoryNameByCategoryId(int? id);

        Task<int?> GetCategoryIdByNameAsync(string categoryName);
    }
}
