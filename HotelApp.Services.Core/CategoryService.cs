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

        public async Task<bool> EditCategoryAsync(CategoryFormInputModel inputModel)
        {
            Category? editableCat = await this.FindCategoryById(inputModel.Id);

            if (editableCat == null)
            {
                return false;
            }

            editableCat.Name = inputModel.Name;
            editableCat.Description = inputModel.Description;
            editableCat.Price = inputModel.Price;
            editableCat.Beds = inputModel.Beds;
            editableCat.ImageUrl = inputModel.ImageUrl;

            await this.dbContext.SaveChangesAsync();

            return true;
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

        public async Task<CategoryDetailsViewModel?> GetCategoryDetailsByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await this.dbContext
                .Categories
                .AsNoTracking()
                .Where(c => c.Id == id.Value)
                .Select(c => new CategoryDetailsViewModel
                {
                    Id = c.Id,
                    Description = c.Description,
                    Name = c.Name,
                    Price = c.Price,
                    Beds = c.Beds,
                    ImageUrl = c.ImageUrl
                })
                .SingleOrDefaultAsync();
        }

        public async Task<CategoryFormInputModel?> GetEditableCategoryByIdAsync(int? id)
        {
            CategoryFormInputModel? editableCat = null;

            if (id.HasValue)
            {
                editableCat = await this.dbContext
                    .Categories
                    .AsNoTracking()
                    .Where(c => c.Id == id.Value)
                    .Select(c => new CategoryFormInputModel()
                    {
                        Description = c.Description,
                        Name = c.Name,
                        Price = c.Price,
                        Beds = c.Beds,
                        ImageUrl = c.ImageUrl
                    })
                    .SingleOrDefaultAsync();
            }

            return editableCat;
        }

        // TODO: Implement as generic method in BaseService
        private async Task<Category?> FindCategoryById(int? id)
        {
            Category? category = null;

            if (id.HasValue)
            {
                category = await this.dbContext
                    .Categories
                    .FindAsync(id.Value);
            }

            return category;
        }


    }
}
