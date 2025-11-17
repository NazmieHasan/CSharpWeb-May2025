namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using HotelApp.Web.ViewModels.Booking;
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
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                BookingManagementDetailsViewModel? bookingDetails = await this.bookingService
                    .GetBookingManagementDetailsByIdAsync(id);

                if (bookingDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(bookingDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            BookingManagementEditFormModel? editFormModel = await this.bookingService
                .GetBookingEditFormModelAsync(id);
            if (editFormModel == null)
            {
                TempData[ErrorMessageKey] = "Selected Booking does not exist!";

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
                if (string.IsNullOrWhiteSpace(inputModel.ManagerEmail))
                {
                    TempData[ErrorMessageKey] = "Please select a manager!";
                    return this.RedirectToAction(nameof(Edit));
                }

                bool success = await this.bookingService
                    .EditBookingAsync(inputModel);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "Error occurred while updating the booking! Ensure to select a valid manager!";
                    return this.RedirectToAction(nameof(Edit));
                }
                else
                {
                    TempData[SuccessMessageKey] = "Booking updated successfully!";
                    return this.RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] =
                    "Unexpected error occurred while editing the booking! Please contact developer team!";

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            Tuple<bool, bool> opResult = await this.bookingService
                .DeleteOrRestoreBookingAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Booking could not be found and updated!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Booking {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }

    }
}
