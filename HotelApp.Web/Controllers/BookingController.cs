namespace HotelApp.Web.Controllers
{
    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;
    using Microsoft.AspNetCore.Mvc;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels;

    public class BookingController : BaseController
    {
        private readonly IRoomService roomService;
        private readonly IBookingService bookingService;

        public BookingController(IRoomService roomService,
            IBookingService bookingService)
        {
            this.roomService = roomService;
            this.bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookingInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }
            
            if (inputModel.DateArrival < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                ModelState.AddModelError(nameof(inputModel.DateArrival), ValidationMessages.Booking.DateArrivalPastMessage);
                return View(inputModel);
            }

            if (inputModel.DateDeparture <= inputModel.DateArrival)
            {
                ModelState.AddModelError(nameof(inputModel.DateDeparture), ValidationMessages.Booking.DateDepartureBeforeArrivalMessage);
                return View(inputModel);
            }

            try
            {
                bool success = await this.bookingService.AddBookingAsync(this.GetUserId()!, inputModel);

                if (!success)
                {
                    TempData[ErrorMessageKey] = ServiceCreateError;
                }
                else
                {
                    TempData[SuccessMessageKey] = "Booking created successfully!";
                }

                return this.RedirectToAction(nameof(My));
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] = ServiceCreateExceptionError;
                return this.RedirectToAction(nameof(My));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                BookingDetailsViewModel? bookingDetails = await this.bookingService
                    .GetBookingDetailsByIdAsync(id);

                if (bookingDetails == null)
                {
                    return this.RedirectToAction(nameof(My));
                }

                return this.View(bookingDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(My));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                EditBookingInputModel? editInputModel = await this.bookingService
                    .GetBookingForEditAsync(id);

                if (editInputModel == null)
                {
                    return this.RedirectToAction(nameof(My));
                }

                return this.View(editInputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(My));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditBookingInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                bool editResult = await this.bookingService
                    .PersistUpdatedBookingAsync(inputModel);

                if (editResult == false)
                {
                    this.ModelState.AddModelError(string.Empty, "Edit error");
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(My));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                DeleteBookingViewModel? bookingToBeDeleted = await this.bookingService
                    .GetBookingDeleteDetailsByIdAsync(id);
                if (bookingToBeDeleted == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(My));
                }

                return this.View(bookingToBeDeleted);
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(My));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteBookingViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    // TODO: Implement JS notifications
                    return this.RedirectToAction(nameof(My));
                }

                bool deleteResult = await this.bookingService
                    .SoftDeleteBookingAsync(inputModel.Id);
                if (deleteResult == false)
                {
                    // TODO: Implement JS notifications
                    // TODO: Alt_Redirect to Not Found page
                    return this.RedirectToAction(nameof(My));
                }

                // TODO: Success notification
                return this.RedirectToAction(nameof(My));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                // TODO: Add JS bars to indicate such errors
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(My));
            }
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            // TODO: Added bookings in list, if booking created with API
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                IEnumerable<MyBookingsViewModel> userBookings = await this.bookingService
                    .GetBookingsByUserIdAsync(userId);
                return View(userBookings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

    }
}
