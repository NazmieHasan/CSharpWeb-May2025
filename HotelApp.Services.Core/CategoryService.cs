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
                    ImageUrl = c.ImageUrl,
                })
                .ToListAsync();

            return allCategories;
        }

    }
}
