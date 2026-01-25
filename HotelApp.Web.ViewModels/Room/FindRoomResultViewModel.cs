namespace HotelApp.Web.ViewModels.Room
{
    public class FindRoomResultViewModel
    {
        public DateOnly DateArrival { get; set; }
        public DateOnly DateDeparture { get; set; }

        public int DaysCount => (DateDeparture.ToDateTime(TimeOnly.MinValue) - DateArrival.ToDateTime(TimeOnly.MinValue)).Days;

        public string? CategoryName { get; set; }

        public List<AllRoomsIndexViewModel> Rooms { get; set; } = new();

        public int FreeCategoryCount => Rooms.Select(r => r.Category).Distinct().Count();
    }
}
