namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;
    using ViewModels.Booking;

    public class BookingApiController : BaseInternalApiController
    {
        private readonly IBookingService bookingService;

        public BookingApiController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("Create")]
        public async Task<ActionResult> Create([FromBody] AddBookingInputModel inputModel)
        {
            string? userId = this.GetUserId();
            bool result = await this.bookingService
                .AddBookingAsync(userId, inputModel.DateArrival.ToString("yyyy-MM-dd"), inputModel.DateDeparture.ToString("yyyy-MM-dd"), inputModel.AdultsCount, inputModel.ChildCount, inputModel.BabyCount);
            if (result == false)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
