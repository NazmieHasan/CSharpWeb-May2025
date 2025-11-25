namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.ManagerManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    using static HotelApp.Web.ViewModels.ValidationMessages.Manager;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Web.ViewModels;

    public class ManagerManagementController : BaseAdminController
    {
        private readonly IManagerManagementService managerService;

        public ManagerManagementController(IManagerManagementService managerService)
        {
            this.managerService = managerService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ManagerManagementIndexViewModel> allManagers = await this.managerService
                .GetManagerManagementBoardDataAsync();

            return View(allManagers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ManagerManagementCreateViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            if (string.IsNullOrWhiteSpace(inputModel.UserEmail))
            {
                ModelState.AddModelError(nameof(inputModel.UserEmail),
                    ValidationMessages.Manager.ManagerEmailRequiredMessage);

                return View(inputModel);
            }

            try
            {
                await this.managerService.AddManagerManagementAsync(inputModel);

                TempData["SuccessMessage"] = "Manager added successfully!";
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(nameof(inputModel.UserEmail), ex.Message);
                return View(inputModel);
            }
            catch (Exception)
            {
                ModelState.AddModelError(nameof(inputModel.UserEmail),
                    ValidationMessages.Manager.ManagerCreateUnexpectedErrorMessage);

                return View(inputModel);
            }
        }

    }
}
