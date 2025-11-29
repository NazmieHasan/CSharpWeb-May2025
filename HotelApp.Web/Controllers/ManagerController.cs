namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Manager;

    public class ManagerController : BaseController
    {
        private readonly IBookingService bookingService;

        public ManagerController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                IEnumerable<ManagerBookingsIndexViewModel> managerBookings = await this.bookingService
                    .GetBookingsByManagerIdAsync(userId);
                return View(managerBookings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> BookingDetails(string? id)
        {
            try
            {
                ManagerBookingDetailsViewModel? bookingDetails = await this.bookingService
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
