namespace HotelApp.Web.Controllers
{
    using HotelApp.Services.Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using HotelApp.Web.ViewModels.Room;

    [AllowAnonymous]
    public class RoomApiController : BaseInternalApiController
    {
        private readonly IRoomService roomService;

        public RoomApiController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("FindFreeRooms")]
        public async Task<IActionResult> FindFreeRoomsByDateArrivalAndDateDeparture([FromQuery] DateOnly dateArrival, [FromQuery] DateOnly dateDeparture)
        {
            if (dateDeparture <= dateArrival || dateArrival < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return BadRequest("Invalid dates.");
            }

            var inputModel = new FindRoomInputModel
            {
                DateArrival = dateArrival,
                DateDeparture = dateDeparture
            };

            var rooms = await roomService.FindRoomByDateArrivaleAndDateDepartureAsync(inputModel);
            return Ok(rooms);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("FindFreeRoomsByCategory")]
        public async Task<IActionResult> FindFreeRoomsByDateArrivalDateDepartureAndCategory([FromQuery] DateOnly dateArrival, [FromQuery] DateOnly dateDeparture, [FromQuery] int categoryId)
        {
            if (dateDeparture <= dateArrival || dateArrival < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return BadRequest("Invalid dates.");
            }

            var inputModel = new FindRoomInputModel
            {
                DateArrival = dateArrival,
                DateDeparture = dateDeparture,
                CategoryId = categoryId
            };

            var rooms = await roomService.FindRoomByDateArrivaleDateDepartureAndCategoryAsync(inputModel);
            return Ok(rooms);
        }
    }
}
