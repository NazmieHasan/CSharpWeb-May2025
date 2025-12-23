namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.CategoryManagement;
    using HotelApp.Web.ViewModels.Admin.RoomManagement;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels;

    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IBookingRepository bookingRepository;

        public CategoryManagementService(ICategoryRepository categoryRepository, IBookingRepository bookingRepository)
        {
            this.categoryRepository = categoryRepository;
            this.bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<CategoryManagementIndexViewModel>> GetCategoryManagementBoardDataAsync()
        {
            return await categoryRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(c => new CategoryManagementIndexViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Beds = c.Beds,
                    IsDeleted = c.IsDeleted
                })
                .ToListAsync()
                ?? Enumerable.Empty<CategoryManagementIndexViewModel>();
        }

        public async Task AddCategoryManagementAsync(CategoryManagementFormInputModel inputModel)
        {
            var existingCategory = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == inputModel.Name.ToLower());

            if (existingCategory != null)
            {
                throw new InvalidOperationException(ValidationMessages.Category.NameAlreadyExistsMessage);
            }

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


        public async Task<CategoryManagementDetailsViewModel?> GetCategoryDetailsByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await this.categoryRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(c => c.Id == id.Value)
                .Select(c => new CategoryManagementDetailsViewModel
                {
                    Id = c.Id,
                    Description = c.Description,
                    Name = c.Name,
                    Price = c.Price,
                    Beds = c.Beds,
                    ImageUrl = c.ImageUrl,
                    IsDeleted = c.IsDeleted,
                    Rooms = c.Rooms
                        .OrderBy(r => r.Name)
                        .Select(r => new RoomInCategoryViewModel
                        {
                            Id = r.Id,
                            Name = r.Name
                        }).ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<CategoryManagementFormInputModel?> GetEditableCategoryByIdAsync(int? id)
        {
            CategoryManagementFormInputModel? editableCat = null;

            if (id.HasValue)
            {
                editableCat = await this.categoryRepository
                .GetAllAttached()
                    .AsNoTracking()
                    .Where(c => c.Id == id.Value)
                    .Select(c => new CategoryManagementFormInputModel()
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

        public async Task<bool> EditCategoryAsync(CategoryManagementFormInputModel inputModel)
        {
            Category? editableCat = await this.FindCategoryById(inputModel.Id);
            if (editableCat == null)
            {
                return false;
            }

            var existingCategory = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.Name.ToLower() == inputModel.Name.ToLower() &&
                    c.Id != inputModel.Id); 

            if (existingCategory != null)
            {
                throw new InvalidOperationException(ValidationMessages.Category.NameAlreadyExistsMessage);
            }

            if (inputModel.Beds < editableCat.Beds)
            {
                bool hasConflictingBookings = await this.bookingRepository
                    .GetAllAttached()
                    .Include(b => b.Room) 
                        .Where(b => b.Room.CategoryId == editableCat.Id)
                        .AnyAsync(b => (b.AdultsCount + b.ChildCount + b.BabyCount) > inputModel.Beds + 1);

                if (hasConflictingBookings)
                {
                    throw new InvalidOperationException(ValidationMessages.Category.BedsCannotBeReducedMessage);
                }
            }

            editableCat.Name = inputModel.Name;
            editableCat.Description = inputModel.Description;
            editableCat.Price = inputModel.Price;
            editableCat.Beds = inputModel.Beds;

            if (!string.IsNullOrEmpty(inputModel.ImageUrl))
            {
                editableCat.ImageUrl = inputModel.ImageUrl;
            }

            bool result = await this.categoryRepository.UpdateAsync(editableCat);
            return result;
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

        public async Task<Tuple<bool, bool>> DeleteOrRestoreCategoryAsync(int? id)
        {
            bool result = false;
            bool isRestored = false;
            if (id > 0)
            {
                Category? category = await this.categoryRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(c => c.Id == id);
                if (category != null)
                {
                    if (category.IsDeleted)
                    {
                        isRestored = true;
                    }

                    category.IsDeleted = !category.IsDeleted;

                    result = await this.categoryRepository
                        .UpdateAsync(category);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
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
