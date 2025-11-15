namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.StatusManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class StatusManagementController : BaseAdminController
    {
        private readonly IStatusManagementService statusService;

        public StatusManagementController(IStatusManagementService statusService)
        {
            this.statusService = statusService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<StatusManagementIndexViewModel> allStatuses = await this.statusService
                .GetStatusManagementBoardDataAsync();

            return View(allStatuses);
        }
    }
}

