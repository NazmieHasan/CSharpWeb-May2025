namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    public class BookingInfoViewModel
    {
        public string BookingId { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateOnly DateArrival { get; set; }

        public DateOnly DateDeparture { get; set; }

        public string Status { get; set; } = null!;
    }
}
