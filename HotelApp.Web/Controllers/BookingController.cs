namespace HotelApp.Web.Controllers
{
    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels.Booking;
    using Microsoft.AspNetCore.Mvc;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels;
    using HotelApp.GCommon;
    using HotelApp.Web.Extensions;
    using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> SaveRoomToSession(string roomId, int adults, int children, int babies)
        {
            var rooms = HttpContext.Session.GetObject<List<PendingBookingRoomSessionModel>>("PendingRooms")
                        ?? new List<PendingBookingRoomSessionModel>();

            if (rooms.Any(r => r.RoomId == roomId))
            {
                return Json(new { success = false, message = "This room is already selected." });
            }

            var roomDetails = await roomService.GetRoomDetailsByIdAsync(roomId);
            if (roomDetails == null)
            {
                return Json(new { success = false, message = "Room not found." });
            }
                
            var pendingRoom = new PendingBookingRoomSessionModel
            {
                RoomId = roomId,
                CategoryName = roomDetails.CategoryName,
                CategoryPrice = roomDetails.CategoryPrice,
                AdultsCount = adults,
                ChildCount = children,
                BabyCount = babies
            };

            rooms.Add(pendingRoom);
            HttpContext.Session.SetObject("PendingRooms", rooms);

            return Json(new { success = true, room = pendingRoom });
        }

        [HttpPost]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult RemoveRoomFromSession(string roomId)
        {
            var rooms = HttpContext.Session.GetObject<List<PendingBookingRoomSessionModel>>("PendingRooms")
                        ?? new List<PendingBookingRoomSessionModel>();

            rooms.RemoveAll(r => r.RoomId == roomId); 
            HttpContext.Session.SetObject("PendingRooms", rooms);

            return Json(new { success = true });
        }

        [HttpGet]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public IActionResult GetRoomsInSession()
        {
            try
            {
                var rooms = HttpContext.Session.GetObject<List<PendingBookingRoomSessionModel>>("PendingRooms")
                            ?? new List<PendingBookingRoomSessionModel>();

                return Json(rooms.Select(r => new
                {
                    roomId = r.RoomId,
                    categoryName = r.CategoryName,
                    categoryPrice = r.CategoryPrice,
                    adultsCount = r.AdultsCount,
                    childCount = r.ChildCount,
                    babyCount = r.BabyCount
                }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add(string dateArrival, string dateDeparture)
        {
            var pendingRooms = HttpContext.Session.GetObject<List<PendingBookingRoomSessionModel>>("PendingRooms");

            if (pendingRooms == null || !pendingRooms.Any())
            {
                Console.WriteLine("Session is empty!");
                return RedirectToAction("Index", "Home");
            }

            if (!DateOnly.TryParse(dateArrival, out var arrivalDate) ||
                !DateOnly.TryParse(dateDeparture, out var departureDate))
            {
                return BadRequest("Invalid dates.");
            }

            var daysCount = (departureDate.ToDateTime(TimeOnly.MinValue) - arrivalDate.ToDateTime(TimeOnly.MinValue)).Days;
            var totalPrice = pendingRooms.Sum(r => r.CategoryPrice * daysCount);

            var model = new AddBookingInputModel
            {
                DateArrival = arrivalDate,
                DateDeparture = departureDate,
                TotalPrice = totalPrice,
                Rooms = pendingRooms.Select(r => new AddBookingRoomInputModel
                {
                    RoomId = Guid.Parse(r.RoomId),
                    AdultsCount = r.AdultsCount,
                    ChildCount = r.ChildCount,
                    BabyCount = r.BabyCount
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookingInputModel inputModel)
        {
            var pendingRooms = HttpContext.Session.GetObject<List<PendingBookingRoomSessionModel>>("PendingRooms");
            
            if (pendingRooms == null || !pendingRooms.Any())
            {
                TempData["ErrorMessage"] = "Please select at least one room.";
                return RedirectToAction(nameof(Index), "Home");
            }

            var daysCount = (inputModel.DateDeparture.ToDateTime(TimeOnly.MinValue) - inputModel.DateArrival.ToDateTime(TimeOnly.MinValue)).Days;

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

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool success = await this.bookingService.AddBookingWithRoomsAsync(this.GetUserId()!, inputModel);

                if (!success)
                {
                    TempData[ErrorMessageKey] = ServiceCreateError;
                }
                else
                {
                    TempData[SuccessMessageKey] = "Booking created successfully!";
                    HttpContext.Session.Remove("PendingRooms");
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
