namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.CategoryManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;
    using static ViewModels.ValidationMessages.Category;

    using static GCommon.ApplicationConstants;

    using System.Collections.Generic;
    using HotelApp.Web.ViewModels.Category;

    public class CategoryManagementController : BaseAdminController
    {
        private readonly ICategoryManagementService categoryService;

        public CategoryManagementController(ICategoryManagementService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryManagementIndexViewModel> allCategories = await this.categoryService
                .GetCategoryManagementBoardDataAsync();

            return View(allCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryManagementFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.categoryService.AddCategoryManagementAsync(inputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                CategoryManagementDetailsViewModel? catDetails = await this.categoryService
                    .GetCategoryDetailsByIdAsync(id);
                if (catDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(catDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                CategoryManagementFormInputModel? editableCat = await this.categoryService
                    .GetEditableCategoryByIdAsync(id);
                if (editableCat == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editableCat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryManagementFormInputModel inputModel)
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
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(int? id)
        {
            Tuple<bool, bool> opResult = await this.categoryService
                .DeleteOrRestoreCategoryAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Category could not be found!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Category {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
