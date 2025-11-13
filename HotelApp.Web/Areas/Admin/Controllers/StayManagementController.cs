namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.StayManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class StayManagementController : BaseAdminController
    {
        private readonly IStayManagementService stayService;

        public StayManagementController(IStayManagementService stayService)
        {
            this.stayService = stayService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<StayManagementIndexViewModel> allStays = await this.stayService
                .GetStayManagementBoardDataAsync();

            return View(allStays);
        }
    }
}
