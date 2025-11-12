namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    using static GCommon.ApplicationConstants;

    public class BookingManagementController : BaseAdminController
    {
        private readonly IBookingManagementService bookingService;
        private readonly IUserManagementService userService;

        public BookingManagementController(IBookingManagementService bookingService, IUserManagementService userService)
        {
            this.bookingService = bookingService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<BookingManagementIndexViewModel> allBookings = await this.bookingService
                .GetBookingManagementBoardDataAsync();

            return View(allBookings);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            BookingManagementEditFormModel? editFormModel = await this.bookingService
                .GetBookingForEditAsync(id);
            if (editFormModel == null)
            {
                TempData[ErrorMessageKey] = "Selected Cinema does not exist!";

                return this.RedirectToAction(nameof(Index));
            }

            editFormModel.AppManagerEmails = await this.userService
                .GetManagerEmailsAsync();

            return this.View(editFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookingManagementEditFormModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool success = await this.bookingService
                    .PersistUpdatedBookingAsync(inputModel);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "Error occurred while updating the booking! Ensure to select a valid data!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Booking updated successfully!";
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] =
                    "Unexpected error occurred while editing the booking! Please contact developer team!";

                return this.RedirectToAction(nameof(Index));
            }
        }

    }
}
