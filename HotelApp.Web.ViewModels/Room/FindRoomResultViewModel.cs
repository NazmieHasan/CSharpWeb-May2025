namespace HotelApp.Web.ViewModels.Room
{
    public class FindRoomResultViewModel
    {
        public DateOnly DateArrival { get; set; }
        public DateOnly DateDeparture { get; set; }

        public string? CategoryName { get; set; }

        public List<AllRoomsIndexViewModel> Rooms { get; set; } = new();
    }
}
