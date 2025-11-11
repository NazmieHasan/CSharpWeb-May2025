namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.CategoryManagement;

    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryManagementService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryManagementIndexViewModel>> GetCategoryManagementBoardDataAsync()
        {
            return await categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(c => new CategoryManagementIndexViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Beds = c.Beds,
                })
                .ToListAsync()
                ?? Enumerable.Empty<CategoryManagementIndexViewModel>();
        }
    }
}
