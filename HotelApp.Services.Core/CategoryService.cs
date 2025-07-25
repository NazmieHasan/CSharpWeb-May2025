namespace HotelApp.Services.Core
{
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Interfaces;
    using Web.ViewModels.Category;

    public class CategoryService : ICategoryService
    {
        private readonly HotelAppDbContext dbContext;

        public CategoryService(HotelAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddCategoryAsync(CategoryFormInputModel inputModel)
        {
            Category newCat = new Category()
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Beds = inputModel.Beds,
                Price = inputModel.Price,
                ImageUrl = inputModel.ImageUrl,
            };

            await this.dbContext.AddAsync(newCat);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AllCategoriesIndexViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<AllCategoriesIndexViewModel> allCategories = await this.dbContext
                .Categories
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
