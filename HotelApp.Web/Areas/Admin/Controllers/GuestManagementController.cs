namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.GuestManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using static GCommon.ApplicationConstants;
    using HotelApp.GCommon;

    using System.Collections.Generic;
    using HotelApp.Web.ViewModels.Admin.BookingManagement;

    public class GuestManagementController : BaseAdminController
    {
        private readonly IGuestManagementService guestService;

        public GuestManagementController(IGuestManagementService guestService)
        {
            this.guestService = guestService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            try
            {
                int pageSize = ApplicationConstants.AdminPaginationPageSize;

                var pagedGuests = await this.guestService
                .GetGuestManagementBoardDataAsync(pageNumber, pageSize);

                int totalGuests = await this.guestService.GetTotalGuestsCountAsync();

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalGuests / pageSize);

                return View(pagedGuests);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }
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
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(nameof(inputModel.Email), ex.Message);
                return this.View(inputModel);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);
                return this.View(inputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                GuestManagementDetailsViewModel? guestDetails = await this.guestService
                    .GetGuestManagementDetailsByIdAsync(id);

                if (guestDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(guestDetails);
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
            GuestManagementEditViewModel? editFormModel = await this.guestService
                .GetGuestEditFormModelAsync(id);
            if (editFormModel == null)
            {
                TempData[ErrorMessageKey] = "Selected Guest does not exist!";

                return this.RedirectToAction(nameof(Index));
            }

            return this.View(editFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GuestManagementEditViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool success = await this.guestService
                    .EditGuestAsync(inputModel);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "Error occurred while updating the guest! Ensure to select a valid manager!";
                    return this.RedirectToAction(nameof(Edit));
                }
                else
                {
                    TempData[SuccessMessageKey] = "Guest updated successfully!";
                    return this.RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(nameof(inputModel.Email), ex.Message);
                return this.View(inputModel);
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] =
                    "Unexpected error occurred while editing the guest! Please contact developer team!";

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            Tuple<bool, bool> opResult = await this.guestService
                .DeleteOrRestoreGuestAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Guest could not be found!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Guest {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
