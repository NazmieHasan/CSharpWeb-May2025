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
    }
}
