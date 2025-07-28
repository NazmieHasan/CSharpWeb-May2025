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

            await this.categoryRepository.AddAsync(newCat);
        }

        public async Task<bool> DeleteCategoryAsync(int? id)
        {
            Category? catToDelete = await this.FindCategoryById(id);
            if (catToDelete == null)
            {
                return false;
            }

            // TODO: To be investigated when relations to Category entity are introduced
            await this.categoryRepository
                .HardDeleteAsync(catToDelete);

            return true;
        }

        public async Task<bool> EditCategoryAsync(CategoryFormInputModel inputModel)
        {
            bool result = false;

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

            result = await this.categoryRepository.UpdateAsync(editableCat);

            return result;
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

        // not added GetCategoriesDropDownDataAsync() to ICategoryRepository because the method
        // use a ViewModel, includes a projection
        // TO DO: Move the projection to the repository as a helper method (but not part of the public interface),
        // which returns IQueryable<Category> or an anonymous DTO,
        // and then apply .Select(...) to a ViewModel in the service layer.
        public async Task<IEnumerable<AddRoomCategoryDropDownModel>> GetCategoriesDropDownDataAsync()
        {
            IEnumerable<AddRoomCategoryDropDownModel> categoriesAsDropDown = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(c => new AddRoomCategoryDropDownModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArrayAsync();

            return categoriesAsDropDown;
        }

        public async Task<DeleteCategoryViewModel?> GetCategoryDeleteDetailsByIdAsync(int? id)
        {
            DeleteCategoryViewModel? deleteCategoryViewModel = null;

            Category? catToBeDeleted = await this.FindCategoryById(id);
            if (catToBeDeleted != null)
            {
                deleteCategoryViewModel = new DeleteCategoryViewModel()
                {
                    Id = catToBeDeleted.Id,
                    Name = catToBeDeleted.Name,
                    ImageUrl = catToBeDeleted.ImageUrl,
                };
            }

            return deleteCategoryViewModel;
        }

        public async Task<CategoryDetailsViewModel?> GetCategoryDetailsByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await this.categoryRepository
                .GetAllAttached()
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
                editableCat = await this.categoryRepository
                .GetAllAttached()
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

        public async Task<bool> SoftDeleteCategoryAsync(int? id)
        {
            bool result = false;

            Category? catToDelete = await this.FindCategoryById(id);
            if (catToDelete == null)
            {
                return false;
            }

            // Soft Delete <=> Edit of IsDeleted property
            result = await this.categoryRepository.DeleteAsync(catToDelete);

            return result;
        }

        // TODO: Implement as generic method in BaseService
        private async Task<Category?> FindCategoryById(int? id)
        {
            Category? category = null;

            if (id.HasValue)
            {
                category = await this.categoryRepository
                    .GetByIdAsync(id.Value);
            }

            return category;
        }


    }
}
