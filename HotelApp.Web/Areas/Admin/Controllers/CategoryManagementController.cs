namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.CategoryManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

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
    }
}
