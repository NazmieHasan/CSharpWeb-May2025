namespace HotelApp.Services.Core
{
    using System.Globalization;

    using Microsoft.EntityFrameworkCore;
    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Category;
    using Web.ViewModels.Room;
    using static GCommon.ApplicationConstants;
    using System.Collections.Generic;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<string?> FindCategoryNameByCategoryId(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }

            return await categoryRepository
                .GetAllAttached()
                .Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<AllCategoriesIndexViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<AllCategoriesIndexViewModel> allCategories = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(c => new AllCategoriesIndexViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Price = c.Price,
                    Beds = c.Beds,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                })
                .ToListAsync();

            return allCategories;
        }

    }
}
