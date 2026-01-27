namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;

    public class BookingApiController : BaseInternalApiController
    {
        private readonly IBookingService bookingService;

        public BookingApiController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("My")]
        public async Task<ActionResult> My()
        {
            string? userId = this.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var bookings = await this.bookingService.GetAllBookingsByUserIdAsync(userId);

            return Ok(bookings);
        }
    }
}
