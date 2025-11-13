namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.ManagerManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

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
    }
}
