namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels;
    using HotelApp.Web.ViewModels.Admin.StayManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    using static HotelApp.Web.ViewModels.ValidationMessages.Stay;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

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

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                StayManagementDetailsViewModel? stayDetails = await this.stayService
                    .GetStayManagementDetailsByIdAsync(id);

                if (stayDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(stayDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(StayManagementEditFormModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool success = await this.stayService
                    .EditStayAsync(inputModel);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "Error occurred while updating the stay! Ensure to select a valid data!";
                    return this.RedirectToAction(nameof(Edit));
                }
                else
                {
                    TempData[SuccessMessageKey] = "Stay updated successfully!";
                    return this.RedirectToAction("Details", "BookingManagement", new { id = inputModel.BookingId });
                }
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] =
                    "Unexpected error occurred while editing the stay! Please contact developer team!";

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            Tuple<bool, bool> opResult = await this.stayService
                .DeleteOrRestoreStayAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Stay could not be found!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Stay {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
