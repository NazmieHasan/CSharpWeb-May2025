namespace HotelApp.WebApi.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class BookingApiController : ControllerBase
    {
        private readonly IBookingService bookingService;

        public BookingApiController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("BookingsByUser")]
        public async Task<ActionResult<IEnumerable<string>>> GetBookingsByUserId([Required] string userId)
        {
            IEnumerable<string> bookingsId = await this.bookingService
                .GetBookingsIdByUserIdAsync(userId);

            return this.Ok(bookingsId);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("Create")]
        public async Task<ActionResult> Create([Required] string roomId, string arrival, string departure, int adultsCount, int childCount, int babyCount)
        {
            string? currentUserId = this.GetUserId();
            bool result = await this.bookingService
                .AddBookingAsync(currentUserId, roomId, arrival, departure, adultsCount, childCount, babyCount);
            if (result == false)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }

        // TODO: Refactor into BaseApiController
        private bool IsUserAuthenticated()
        {
            bool retRes = false;
            if (this.User.Identity != null)
            {
                retRes = this.User.Identity.IsAuthenticated;
            }

            return retRes;
        }

        private string? GetUserId()
        {
            string? userId = null;
            if (this.IsUserAuthenticated())
            {
                userId = this.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return userId;
        }
    }
}
