namespace HotelApp.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;


    using HotelApp.Web.ViewModels.Admin.CategoryManagement;
    using Services.Core.Admin.Interfaces;

    using static ViewModels.ValidationMessages.Category;
    using static GCommon.ApplicationConstants;

    public class CategoryManagementController : BaseAdminController
    {
        private readonly ICategoryManagementService categoryService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoryManagementController(ICategoryManagementService categoryService, IWebHostEnvironment webHostEnvironment)
        {
            this.categoryService = categoryService;
            this.webHostEnvironment = webHostEnvironment;
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

            if (inputModel.Image == null || inputModel.Image.Length == 0)
            {
                this.ModelState.AddModelError(nameof(inputModel.Image), "Please, upload image");
                return this.View(inputModel);
            }

            if (inputModel.Image.Length > 2 * 1024 * 1024)
            {
                this.ModelState.AddModelError(nameof(inputModel.Image), "Max size is 2MB");
                return this.View(inputModel);
            }

            string ext = Path.GetExtension(inputModel.Image.FileName).ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
            {
                this.ModelState.AddModelError(nameof(inputModel.Image), "Allowed files formats are jpg, jpeg, png");
                return this.View(inputModel);
            }

            try
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/upload/categories");
                Directory.CreateDirectory(uploadsFolder);
                string fileName = Guid.NewGuid() + ext;
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await inputModel.Image.CopyToAsync(fileStream);
                }

                inputModel.ImageUrl = "/images/upload/categories/" + fileName;

                await this.categoryService.AddCategoryManagementAsync(inputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(nameof(inputModel.Name), ex.Message);
                return this.View(inputModel);
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
            if (!ModelState.IsValid)
                return View(inputModel);

            try
            {
                if (inputModel.Image != null && inputModel.Image.Length > 0)
                {
                    if (inputModel.Image.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError(nameof(inputModel.Image), "Max size is 2MB");
                        return View(inputModel);
                    }

                    string ext = Path.GetExtension(inputModel.Image.FileName).ToLower();
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    {
                        ModelState.AddModelError(nameof(inputModel.Image), "Allowed file formats: jpg, jpeg, png");
                        return View(inputModel);
                    }

                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/upload/categories");
                    Directory.CreateDirectory(uploadsFolder);
                    string fileName = Guid.NewGuid() + ext;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await inputModel.Image.CopyToAsync(fileStream);
                    }

                    inputModel.ImageUrl = "/images/upload/categories/" + fileName;
                }
                else
                {
                    inputModel.ImageUrl = null; 
                }

                bool editSuccess = await categoryService.EditCategoryAsync(inputModel);
                if (!editSuccess)
                    return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(inputModel.Name), ex.Message);
                return View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                return View(inputModel);
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
