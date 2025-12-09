namespace HotelApp.Web.Controllers
{
    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels.Booking;
    using Microsoft.AspNetCore.Mvc;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels;
    using HotelApp.GCommon;

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
        public async Task<IActionResult> Add(string roomId, DateOnly dateArrival, DateOnly dateDeparture)
        {
            var roomDetails = await this.roomService.GetRoomDetailsByIdAsync(roomId);

            if (roomDetails == null)
            {
                return NotFound();
            }

            var model = new AddBookingInputModel
            {
                RoomId = Guid.Parse(roomId),
                DateArrival = dateArrival,
                DateDeparture = dateDeparture,
                MaxGuests = roomDetails.CategoryBeds 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookingInputModel inputModel)
        {
            var roomDetails = await this.roomService.GetRoomDetailsByIdAsync(inputModel.RoomId.ToString());
            if (roomDetails == null)
            {
                return NotFound();
            }

            inputModel.MaxGuests = roomDetails.CategoryBeds;

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
        public async Task<IActionResult> My(int pageNumber = 1)
        {
            // TODO: Added bookings in list, if booking created with API
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                int pageSize = ApplicationConstants.MyBookingsPaginationPageSize;

                IEnumerable<MyBookingsViewModel> userBookings = await this.bookingService
                    .GetBookingsByUserIdAsync(userId, pageNumber, pageSize);

                int totalBookings = await this.bookingService.GetBookingsCountByUserIdAsync(userId);

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalBookings / pageSize);

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
