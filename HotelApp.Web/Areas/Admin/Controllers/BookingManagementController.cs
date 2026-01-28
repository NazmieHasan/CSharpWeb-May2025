namespace HotelApp.Web.Areas.Admin.Controllers
{
    using Rotativa.AspNetCore;

    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using HotelApp.Web.ViewModels.Admin.BookingManagement.Report;
    using HotelApp.Web.ViewModels.Admin.BookingManagement.Search;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using static GCommon.ApplicationConstants;

    public class BookingManagementController : BaseAdminController
    {
        private readonly IBookingManagementService bookingService;
        private readonly IUserManagementService userService;
        private readonly IStatusManagementService statusService;

        public BookingManagementController(IBookingManagementService bookingService,
            IUserManagementService userService,
            IStatusManagementService statusService)
        {
            this.bookingService = bookingService;
            this.userService = userService;
            this.statusService = statusService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            try
            {
                int pageSize = ApplicationConstants.AdminPaginationPageSize;

                var pagedBookings = await bookingService
                    .GetBookingManagementBoardDataAsync(pageNumber, pageSize);

                int totalBookings = await bookingService.GetTotalBookingsCountAsync();

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalBookings / pageSize);

                return View(pagedBookings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }
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

            editFormModel.Statuses = await this.statusService.GetAllowedStatusesInBookingEditAsync(editFormModel.StatusId, editFormModel.DateDeparture, editFormModel.Id);

            return this.View(editFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookingManagementEditFormModel inputModel)
        {
            BookingManagementEditFormModel? booking = await this.bookingService
                .GetBookingEditFormModelAsync(inputModel.Id);

            if (booking == null)
            {
                TempData[ErrorMessageKey] = "Selected Booking does not exist!";
                return RedirectToAction(nameof(Index));
            }

            inputModel.AppManagerEmails = await this.userService.GetManagerEmailsAsync();

            inputModel.Statuses = await this.statusService
                .GetAllowedStatusesInBookingEditAsync(booking.StatusId, booking.DateDeparture, booking.Id);

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                if (string.IsNullOrWhiteSpace(inputModel.ManagerEmail))
                {
                    ModelState.AddModelError(nameof(inputModel.ManagerEmail), "Please select a manager!");
                    return View(inputModel);
                }

                if (!inputModel.Statuses.Any(s => s.Id == inputModel.StatusId))
                {
                    ModelState.AddModelError(nameof(inputModel.StatusId), "Selected status is not allowed for the current booking state.");
                    return View(inputModel);
                }

                bool success = await this.bookingService
                    .EditBookingAsync(inputModel);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "Error occurred while updating the booking! Ensure to select a valid manager!";
                    return RedirectToAction(nameof(Edit), new { id = inputModel.Id });
                }
                else
                {
                    TempData[SuccessMessageKey] = "Booking updated successfully!";
                    return RedirectToAction(nameof(Details), new { id = inputModel.Id });
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

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var model = new BookingManagementSearchViewModel
            {
                Search =
                {
                    Statuses = await statusService.GetBookingStatusesDropDownDataAsync(),
                },
                HasSearched = false
            }; 

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchResult(BookingManagementSearchViewModel model)
        {
            model.Results = await bookingService.SearchBookingAsync(model.Search);
            model.Search.Statuses = await statusService.GetBookingStatusesDropDownDataAsync();
            model.HasSearched = true;

            return View("Search", model);
        }

        [HttpGet]
        public async Task<IActionResult> ReportRevenue()
        {
            var model = new BookingManagementReportRevenueSearchViewModel
            {
                HasReportSearched = false
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ReportRevenueResult(BookingManagementReportRevenueSearchViewModel model)
        {
            var bookings = await bookingService.ReportBookingRevenueAsync(model.ReportSearch);

            model.TotalRevenue = bookings.Sum(b => b.Status == "Cancelled" ? b.PaidAmount / 2 : b.PaidAmount);
            model.ReportResults = bookings;
            model.HasReportSearched = true;

            return View("ReportRevenue", model);
        }

        [HttpGet]
        public async Task<IActionResult> ReportGuestCount()
        {
            var model = new BookingManagementReportGuestCountSearchViewModel
            {
                HasReportSearched = false
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ReportGuestCountResult(BookingManagementReportGuestCountSearchViewModel model)
        {
            var bookings = await bookingService.ReportBookingGuestCountAsync(model.ReportSearch);

            model.ReportResults = bookings;
            model.HasReportSearched = true;

            return View("ReportGuestCount", model);
        }


        [HttpGet]
        public async Task<IActionResult> ReportRevenuePdf(int year, int month)
        {
            var reportSearchModel = new BookingManagementReportRevenuePdf
            {
                ReportSearch = new BookingManagementReportSearchInputModel
                {
                    Year = year,
                    Month = month
                },
                Pdf =
				{
					PdfOwnerUser = User.Identity!.Name,
					PdfGeneratedOn = DateTime.Now.ToString(AppDateTimeFormat)
				}
            };

            var bookings = await bookingService.ReportBookingRevenueAsync(reportSearchModel.ReportSearch);
            reportSearchModel.ReportResults = bookings.ToList();

            reportSearchModel.TotalRevenue = bookings.Sum(b => b.Status == "Cancelled" ? b.PaidAmount / 2 : b.PaidAmount);
            reportSearchModel.HasReportSearched = true;

            return new ViewAsPdf("ReportRevenuePdf", reportSearchModel)
            {
                FileName = $"BookingRevenue_{year}-{month}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),
                CustomSwitches =
                    "--no-pdf-compression " +
                    "--disable-smart-shrinking " +
                    "--print-media-type " +
                    "--footer-center \"Page [page] of [topage]\" " +
                    "--footer-font-size 8 " +
                    "--footer-spacing 4 "
            };
        }

        [HttpGet]
        public async Task<IActionResult> ReportGuestCountPdf(int year, int month)
        {
            var reportSearchModel = new BookingManagementReportGuestCountPdf
            {
                ReportSearch = new BookingManagementReportSearchInputModel
                {
                    Year = year,
                    Month = month
                },
                Pdf =
				{
					PdfOwnerUser = User.Identity!.Name,
					PdfGeneratedOn = DateTime.Now.ToString(AppDateTimeFormat)
				}
            };

            reportSearchModel.ReportResults = (await bookingService.ReportBookingGuestCountAsync(reportSearchModel.ReportSearch)).ToList();
            reportSearchModel.HasReportSearched = true;

            return new ViewAsPdf("ReportGuestCountPdf", reportSearchModel)
            {
                FileName = $"BookingGuestCount_{year}-{month}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),
                CustomSwitches =
                    "--no-pdf-compression " +
                    "--disable-smart-shrinking " +
                    "--print-media-type " +
                    "--footer-center \"Page [page] of [topage]\" " +
                    "--footer-font-size 8 " +
                    "--footer-spacing 4 "
            };
        }

    }
}
