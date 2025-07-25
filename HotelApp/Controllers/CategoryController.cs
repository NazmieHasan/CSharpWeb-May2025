namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;
    using ViewModels.Category;
    using static ViewModels.ValidationMessages.Category;

    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllCategoriesIndexViewModel> allCategories = await this.categoryService
                .GetAllCategoriesAsync();

            return View(allCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.categoryService.AddCategoryAsync(inputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                CategoryDetailsViewModel? catDetails = await this.categoryService
                    .GetCategoryDetailsByIdAsync(id);
                if (catDetails == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(catDetails);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                CategoryFormInputModel? editableCat = await this.categoryService
                    .GetEditableCategoryByIdAsync(id);
                if (editableCat == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editableCat);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool editSuccess = await this.categoryService.EditCategoryAsync(inputModel);
                if (!editSuccess)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                DeleteCategoryViewModel? catToBeDeleted = await this.categoryService
                    .GetCategoryDeleteDetailsByIdAsync(id);
                if (catToBeDeleted == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(catToBeDeleted);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    // TODO: Implement JS notifications
                    return this.RedirectToAction(nameof(Index));
                }

                bool deleteResult = await this.categoryService
                    .SoftDeleteCategoryAsync(inputModel.Id);
                if (deleteResult == false)
                {
                    // TODO: Implement JS notifications
                    // TODO: Alt_Redirect to Not Found page
                    return this.RedirectToAction(nameof(Index));
                }

                // TODO: Success notification
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }


    }
}
