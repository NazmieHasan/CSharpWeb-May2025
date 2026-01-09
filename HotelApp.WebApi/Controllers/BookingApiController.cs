namespace HotelApp.WebApi.Controllers
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;

    public class BookingApiController : BaseExternalApiController
    {
        private readonly IBookingService bookingService;

        public BookingApiController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        // TODO fix Status 401 Unauthorized

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("BookingsByUser")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetBookingsByUserId([Required] string userId)
        {
            IEnumerable<string> bookingsId = await this.bookingService
                .GetBookingsIdByUserIdAsync(userId);

            return this.Ok(bookingsId);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("Create")]
        [Authorize]
        public async Task<ActionResult> Create(string arrival, string departure, int adultsCount, int childCount, int babyCount)
        {
            string? currentUserId = this.GetUserId();
            bool result = await this.bookingService
                .AddBookingAsync(currentUserId, arrival, departure);
            if (result == false)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }

    }
}
