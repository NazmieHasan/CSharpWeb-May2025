namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.GuestManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class GuestManagementController : BaseAdminController
    {
        private readonly IGuestService guestService;

        public GuestManagementController(IGuestService guestService)
        {
            this.guestService = guestService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GuestManagementIndexViewModel> allGuests = await this.guestService
                .GetGuestManagementBoardDataAsync();

            return View(allGuests);
        }
    }
}
