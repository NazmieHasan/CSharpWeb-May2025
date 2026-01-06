namespace HotelApp.Web.ViewModels.Manager
{
    public class ManagerBookingDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateOnly DateArrival { get; set; }

        public DateOnly DateDeparture { get; set; }
    }
}
