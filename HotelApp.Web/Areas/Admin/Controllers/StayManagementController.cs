namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels;
    using HotelApp.Web.ViewModels.Admin.StayManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    using static HotelApp.Web.ViewModels.ValidationMessages.Stay;

    public class StayManagementController : BaseAdminController
    {
        private readonly IStayManagementService stayService;
        private readonly IBookingManagementService bookingService;

        public StayManagementController(IStayManagementService stayService, IBookingManagementService bookingService)
        {
            this.stayService = stayService;
            this.bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<StayManagementIndexViewModel> allStays = await this.stayService
                .GetStayManagementBoardDataAsync();

            return View(allStays);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid bookingId)
        {
            var booking = await this.bookingService.FindBookingByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var model = new StayManagementCreateViewModel
            {
                BookingId = bookingId
            };

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StayManagementCreateViewModel inputModel)
        {
            var booking = await this.bookingService.FindBookingByIdAsync(inputModel.BookingId);

            if (booking == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_Create", inputModel);
            }

            if (inputModel.GuestEmail == null)
            {
                ModelState.AddModelError(nameof(inputModel.GuestEmail),
                    ValidationMessages.Stay.GuestEmailRequiredMessage);

                return PartialView("_Create", inputModel);
            }

            try
            {
                await this.stayService.AddStayManagementAsync(inputModel);

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Details", "BookingManagement", new { id = inputModel.BookingId })
                });
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(inputModel.GuestEmail), e.Message);
                return PartialView("_Create", inputModel);
            }
        }
    }
}
