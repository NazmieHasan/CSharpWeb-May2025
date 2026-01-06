namespace HotelApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using HotelApp.GCommon;
    using HotelApp.Services.Core.Admin.Interfaces;
    using HotelApp.Web.ViewModels.Admin.BookingRoomManagement;
    
    using static GCommon.ApplicationConstants;

    public class BookingRoomManagementController : BaseAdminController
    {
        private readonly IBookingRoomManagementService bookingRoomService;
        private readonly IStatusManagementService statusService;

        public BookingRoomManagementController(IBookingRoomManagementService bookingRoomService, 
            IStatusManagementService statusService)
        {
            this.bookingRoomService = bookingRoomService;
            this.statusService = statusService; 
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            try
            {
                int pageSize = ApplicationConstants.AdminPaginationPageSize;

                var pagedBookingRooms = await bookingRoomService
                .GetBookingRoomManagementBoardDataAsync(pageNumber, pageSize);

                int totalBookingRooms = await bookingRoomService.GetTotalBookingRoomsCountAsync();

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalBookingRooms / pageSize);

                return View(pagedBookingRooms);
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
                BookingRoomManagementDetailsViewModel? bookingRoomDetails = await this.bookingRoomService
                    .GetBookingRoomManagementDetailsByIdAsync(id);

                if (bookingRoomDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(bookingRoomDetails);
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
            BookingRoomManagementEditFormModel? editFormModel = await this.bookingRoomService
                .GetBookingRoomEditFormModelAsync(id);
            if (editFormModel == null)
            {
                TempData[ErrorMessageKey] = "Selected Booking Room does not exist!";

                return this.RedirectToAction(nameof(Index));
            }

            editFormModel.Statuses = await this.statusService.GetAllowedStatusesInBookingRoomEditAsync(editFormModel.StatusId, editFormModel.DateDeparture, editFormModel.Id);

            return this.View(editFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookingRoomManagementEditFormModel inputModel)
        {
            BookingRoomManagementEditFormModel? bookingRoom = await this.bookingRoomService
                .GetBookingRoomEditFormModelAsync(inputModel.Id);

            if (bookingRoom == null)
            {
                TempData[ErrorMessageKey] = "Selected Booking Room does not exist!";
                return RedirectToAction(nameof(Index));
            }

            inputModel.Statuses = await this.statusService
                .GetAllowedStatusesInBookingRoomEditAsync(bookingRoom.StatusId, bookingRoom.DateDeparture, bookingRoom.Id);

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                if (!inputModel.Statuses.Any(s => s.Id == inputModel.StatusId))
                {
                    ModelState.AddModelError(nameof(inputModel.StatusId), "Selected status is not allowed for the current booking room state.");
                    return View(inputModel);
                }

                bool success = await this.bookingRoomService
                    .EditBookingRoomAsync(inputModel);
                if (!success)
                {
                    TempData[ErrorMessageKey] = "Error occurred while updating the booking room! Ensure to select a valid data!";
                    return RedirectToAction(nameof(Edit), new { id = inputModel.Id });
                }
                else
                {
                    TempData[SuccessMessageKey] = "Booking room updated successfully!";
                    return RedirectToAction(nameof(Details), new { id = inputModel.Id });
                }
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] =
                    "Unexpected error occurred while editing the booking room! Please contact developer team!";

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            Tuple<bool, bool> opResult = await this.bookingRoomService
                .DeleteOrRestoreBookingRoomAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Booking room could not be found and updated!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Booking room {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }

    }
}
