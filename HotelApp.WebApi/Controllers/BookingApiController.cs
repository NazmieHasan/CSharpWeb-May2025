namespace HotelApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;

    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;
    
    public class BookingApiController : BaseExternalApiController
    {
        private readonly IBookingService bookingService;
        private readonly IRoomService roomService;
        private ICategoryService categoryService;

        public BookingApiController(IBookingService bookingService, IRoomService roomService, ICategoryService categoryService)
        {
            this.bookingService = bookingService;
            this.roomService = roomService;
            this.categoryService = categoryService;
        }

        [HttpPost]
        [Route("CreateBooking")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBooking([FromBody] AddBookingInputModel inputModel)
        {
            // TODO: Move all booking validation and business logic (date checks, room availability, guest limits, price calculation) into the BookingService.

            string? userId = this.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (inputModel.DateDeparture <= inputModel.DateArrival)
                return BadRequest("DateDeparture must be later than DateArrival.");

            if (inputModel.Owner == "string")
                return BadRequest("Owner name is required.");

            List<AllRoomsIndexViewModel> freeRooms;

            if ((!string.IsNullOrWhiteSpace(inputModel.RoomCategoryName)) && inputModel.RoomCategoryName != "string")
            {
                int? categoryId = await this.categoryService.GetCategoryIdByNameAsync(inputModel.RoomCategoryName);

                if (!categoryId.HasValue)
                    return BadRequest("Invalid room category.");

                freeRooms = (await this.roomService
                    .FindRoomByDateArrivaleDateDepartureAndCategoryAsync(
                        new FindRoomInputModel
                        {
                            DateArrival = inputModel.DateArrival,
                            DateDeparture = inputModel.DateDeparture,
                            CategoryId = categoryId.Value
                        }))
                    .ToList();
            }
            else
            {
                freeRooms = (await this.roomService.FindRoomByDateArrivaleAndDateDepartureAsync(
                    new FindRoomInputModel
                    {
                        DateArrival = inputModel.DateArrival,
                        DateDeparture = inputModel.DateDeparture
                    })).ToList();
            }

            if (!freeRooms.Any())
                return BadRequest("No rooms are available for the selected dates.");

            var requestedRoomIds = inputModel.Rooms.Select(r => r.RoomId.ToString()).ToList();
            foreach (var requestedRoom in inputModel.Rooms)
            {
                var room = freeRooms.FirstOrDefault(r => r.Id == requestedRoom.RoomId.ToString());

                if (room == null)
                {
                    if (!string.IsNullOrWhiteSpace(inputModel.RoomCategoryName) &&
                        inputModel.RoomCategoryName != "string")
                    {
                        return BadRequest(
                            "Selected room does not belong to the chosen category."
                        );
                    }

                    return BadRequest(
                        "Selected room is not available for the selected dates."
                    );
                }
            }

            foreach (var requestedRoom in inputModel.Rooms)
            {
                var room = freeRooms.First(r => r.Id == requestedRoom.RoomId.ToString());

                int adultsAndChildren =
                    requestedRoom.AdultsCount +
                    requestedRoom.ChildCount;

                int babies = requestedRoom.BabyCount;
                int beds = room.CategoryBeds;

                bool isValid =
                    // Case 1: if adults + children == beds, then allowed max 1 baby
                    (adultsAndChildren == beds && babies <= 1)

                    // Case 2: if adults + children < beds, then allowed total guests <= beds + 1
                    || (adultsAndChildren < beds && adultsAndChildren + babies <= beds + 1);

                if (!isValid)
                {
                    return BadRequest(
                        $"Room '{room.Name}' allows maximum {beds} adults/children " +
                        $"and up to {beds + 1} guests including babies."
                    );
                }
            }

            var daysCount = (inputModel.DateDeparture.ToDateTime(TimeOnly.MinValue) - inputModel.DateArrival.ToDateTime(TimeOnly.MinValue)).Days;
            var totalPrice = freeRooms
                .Where(r => requestedRoomIds.Contains(r.Id))
                .Sum(r => r.CategoryPrice * daysCount);
            inputModel.TotalPrice = totalPrice;

            var bookingSuccess = await this.bookingService.AddBookingWithRoomsAsync(userId, inputModel);

            if (!bookingSuccess)
                return BadRequest("Invalid booking data or user not found.");

            return CreatedAtAction(nameof(MyBookings), null, new { inputModel });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("MyBookings")]
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

    }
}
