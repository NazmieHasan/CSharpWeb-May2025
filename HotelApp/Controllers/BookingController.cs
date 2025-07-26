namespace HotelApp.Web.Controllers
{
    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;
    using Microsoft.AspNetCore.Mvc;

    using static ViewModels.ValidationMessages.Booking;

    public class BookingController : Controller
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
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<AllBookingsIndexViewModel> allBookings = await
                    this.bookingService.GetAllBookingsAsync();

                return View(allBookings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookingInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.bookingService.AddBookingAsync(inputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
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
                    return this.RedirectToAction(nameof(Index), "Home");
                }

                return this.View(bookingDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }

        }

    }
}
