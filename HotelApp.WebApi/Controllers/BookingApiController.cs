namespace HotelApp.WebApi.Controllers
{
    using HotelApp.Web.ViewModels.Booking;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;

    public class BookingApiController : BaseExternalApiController
    {
        private readonly IBookingService bookingService;

        public BookingApiController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("BookingsByUser")]
        public async Task<IActionResult> MyBookings()
        {
            string? userId = this.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            IEnumerable<MyBookingsViewModel> bookings =
                await this.bookingService.GetAllBookingsByUserIdAsync(userId);

            return Ok(bookings);
        }


        // TODO create booking

    }
}
