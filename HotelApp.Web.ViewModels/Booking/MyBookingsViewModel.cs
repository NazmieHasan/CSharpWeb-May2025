namespace HotelApp.Web.ViewModels.Booking
{
    public class MyBookingsViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateOnly DateArrival { get; set; }

        public DateOnly DateDeparture { get; set; }

        public int TotalAdultsCount { get; set; }

        public int TotalChildCount { get; set; }

        public int TotalBabyCount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }

        public string Status { get; set; } = null!;

        public IEnumerable<RoomInfoInMyBookingViewModel> Rooms { get; set; } = new List<RoomInfoInMyBookingViewModel>();
    }
}
