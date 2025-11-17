namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.GuestManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class GuestManagementController : BaseAdminController
    {
        private readonly IGuestManagementService guestService;

        public GuestManagementController(IGuestManagementService guestService)
        {
            this.guestService = guestService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GuestManagementIndexViewModel> allGuests = await this.guestService
                .GetGuestManagementBoardDataAsync();

            return View(allGuests);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GuestManagementCreateViewModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.guestService.AddGuestManagementAsync(inputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);
                return this.View(inputModel);
            }
        }
    }
}
